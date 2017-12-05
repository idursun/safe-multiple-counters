open System.IO

open System.Net

open Suave
open Suave.Operators
open Suave.Json
open System.Runtime.Serialization

let path = Path.Combine("src","Client") |> Path.GetFullPath
let port = 8085us

let config =
  { defaultConfig with 
      homeFolder = Some path
      bindings = [ HttpBinding.create HTTP (IPAddress.Parse "0.0.0.0") port ] }

let init =
  42
  |> string
  |> Successful.OK

[<DataContract>]
type SomeData = { 
  name: string; 
  age: int
}

let somedata = { name = "name goes here"; age = 32 }

let somedataHandler =
  somedata
  |> toJson
  |> string
  |> Successful.OK



let webpart = 
  choose [
    Filters.path "/api/init" >=> init
    Filters.path "/api/somedata" >=> somedataHandler
    Files.browseHome
  ]

startWebServer config webpart
