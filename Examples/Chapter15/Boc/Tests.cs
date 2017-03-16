using Boc.Chapter10.Domain;
using NUnit.Framework;
using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using Boc.Commands;
using Microsoft.AspNetCore.Mvc;
using Boc.Domain;

namespace Boc.Chapter15
{
   public class Tests
   {
      [Test]
      public void WhenAccountDoesntExist_Then400()
      {
         var registry = new AccountRegistry(
            loadAccount: _ => Async((Option<AccountState>)None),
            saveAndPublish: _ => Async(Unit()));

         var controller = new MakeTransferController(
            validate: cmd => Valid(cmd),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer()).Result;

         Assert.AreEqual(typeof(BadRequestObjectResult), response.GetType());
      }

      [Test]
      public void WhenValidationFails_Then400()
      {
         bool changesPersisted = false;

         var registry = new AccountRegistry(
            loadAccount: _ => Async(Some(new AccountState(Currency: "EUR"))),
            saveAndPublish: _ =>
            {
               changesPersisted = true;
               return Async(Unit());
            });

         var controller = new MakeTransferController(
            validate: cmd => Invalid("invalid"),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer()).Result;

         Assert.IsFalse(changesPersisted);
         Assert.AreEqual(typeof(BadRequestObjectResult), response.GetType());
      }

      [Test]
      public void WhenInsufficientBalance_Then400()
      {
         bool changesPersisted = false;

         var accountState = new AccountState
         (
            Currency: "EUR",
            Balance: 1000,
            Status: AccountStatus.Active
         );

         var registry = new AccountRegistry(
            loadAccount: _ => Async(Some(accountState)),
            saveAndPublish: _ =>
            {
               changesPersisted = true;
               return Async(Unit());
            });

         var controller = new MakeTransferController(
            validate: cmd => Invalid("invalid"),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer
         {
            Amount = 1200
         }).Result;

         Assert.IsFalse(changesPersisted);
         Assert.AreEqual(typeof(BadRequestObjectResult), response.GetType());
      }

      [Test]
      public void WhenEverythingWorks_Then200()
      {
         bool changesPersisted = false;

         var accountState = new AccountState
         (
            Currency: "EUR",
            Balance: 1000,
            Status: AccountStatus.Active
         );

         var registry = new AccountRegistry(
            loadAccount: _ => Async(Some(accountState)),
            saveAndPublish: _ =>
            {
               changesPersisted = true;
               return Async(Unit());
            });

         var controller = new MakeTransferController(
            validate: cmd => Valid(cmd),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer
         {
            Amount = 200
         }).Result;

         Assert.IsTrue(changesPersisted);
         Assert.AreEqual(typeof(OkObjectResult), response.GetType());
      }

      [Test]
      public void WhenLoadingFails_Then500()
      {
         bool changesPersisted = false;

         var accountState = new AccountState
         (
            Currency: "EUR",
            Balance: 1000,
            Status: AccountStatus.Active
         );

         var registry = new AccountRegistry(
            loadAccount: _ => { throw new InvalidOperationException(); },
            saveAndPublish: _ =>
            {
               changesPersisted = true;
               return Async(Unit());
            });

         var controller = new MakeTransferController(
            validate: cmd => Valid(cmd),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer
         {
            Amount = 200
         }).Result;

         Assert.IsFalse(changesPersisted);
         Assert.AreEqual(typeof(ObjectResult), response.GetType());
      }

      [Test]
      public void AccountIsOnlyLoadedOnce()
      {
         int accountLoaded = 0;
         int changesPersisted = 0;

         var accountState = new AccountState
         (
            Currency: "EUR",
            Balance: 1000,
            Status: AccountStatus.Active
         );

         var registry = new AccountRegistry(
            loadAccount: _ => 
            {
               accountLoaded++;
               return Async(Some(accountState));
            },
            saveAndPublish: _ =>
            {
               changesPersisted++;
               return Async(Unit());
            });

         var controller = new MakeTransferController(
            validate: Valid,
            getAccount: id => registry.Lookup(id));

         // make 2 transfers
         var cmd = new MakeTransfer { Amount = 200 };
         var x = controller.MakeTransfer(cmd).Result;
         var y = controller.MakeTransfer(cmd).Result;

         Assert.AreEqual(2, changesPersisted);
         Assert.AreEqual(1, accountLoaded);
      }

      [Test]
      public void WhenPersistenceFails_Then500()
      {
         var accountState = new AccountState
         (
            Currency: "EUR",
            Balance: 1000,
            Status: AccountStatus.Active
         );

         var registry = new AccountRegistry(
            loadAccount: _ => Async(Some(accountState)),
            saveAndPublish: _ => { throw new InvalidOperationException(); });

         var controller = new MakeTransferController(
            validate: cmd => Valid(cmd),
            getAccount: id => registry.Lookup(id));

         var response = controller.MakeTransfer(new MakeTransfer
         {
            Amount = 200
         }).Result;

         Assert.AreEqual(typeof(ObjectResult), response.GetType());
      }
   }
}
