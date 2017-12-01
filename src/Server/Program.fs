open System.IO

open System.Net

open Suave
open Suave.Operators

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


let webpart = 
  choose [
    Filters.path "/api/init" >=> init
    Files.browseHome
  ]

startWebServer config webpart
