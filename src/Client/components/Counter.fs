module Counter

open Elmish
open Fable.Helpers.React.Props
module R = Fable.Helpers.React

type Model = int

type Msg = Increment | Decrement

let init () = 0

let update msg model =
    let model' = match msg  with
                 |Increment -> model + 1
                 |Decrement -> model - 1
    model', Cmd.none


let view model dispatch =
  R.div [ClassName "field is-grouped has-addons"]
      [ 
          R.p [ClassName "control"] [R.a [ ClassName "button"; OnClick (fun _ -> dispatch Decrement) ] [ R.str "-" ] ]
          R.p [ClassName "control"] [R.p [] [R.str (sprintf "%A" model)]]
          R.p [ClassName "control"] [R.a [ ClassName "button"; OnClick (fun _ -> dispatch Increment) ] [ R.str "+" ] ]
      ]
