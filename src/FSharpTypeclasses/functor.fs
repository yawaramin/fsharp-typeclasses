namespace FsharpTypeclasses

[<RequireQualifiedAccess>]
module Functor =
  type t<'a, 'b, 'fa, 'fb> = { map : ('a -> 'b) -> 'fa -> 'fb }

  let option = { map = Option.map }
  let list = { map = List.map }
  let array = { map = Array.map }
  let tuple2 = { map = fun f (a1, a2) -> (f a1, f a2) }

  module Ops = let map t = t.map
  module Laws =
    open Eq.Ops
    open Ops

    let identity t eq_t fa = eq eq_t fa <| map t id fa
    let composition t1 t2 t3 eq_t f g fa =
      let lhs = map t2 g <| map t1 f fa
      let rhs = map t3 (g << f) fa

      eq eq_t lhs rhs
