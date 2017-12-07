module Components.Footer

module R = Fable.Helpers.React

open Fulma.Layouts

let footerView _ _ =
  Footer.footer [] [
    R.str "footer goes here"
  ]