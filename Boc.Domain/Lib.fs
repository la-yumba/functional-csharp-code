namespace Boc.Domain.FSharp
open System

type AccountStatus =
   Requested | Active | Frozen | Dormant | Closed

type CurrencyCode = string

type Transaction = {
   Amount: decimal
   Description: string
   Date: DateTime
}

type AccountState = {
   Status: AccountStatus
   Currency: CurrencyCode
   AllowedOverdraft: decimal
   TransactionHistory: Transaction list
} 

type AccountState with

member this.WithStatus(status) =
   { this with Status = Active }

member this.AddTransaction(trans) =
   { this with TransactionHistory = trans :: 
                                    this.TransactionHistory}
