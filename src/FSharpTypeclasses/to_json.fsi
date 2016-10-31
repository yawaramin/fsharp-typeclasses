namespace FsharpTypeclasses

open System.Collections.Generic

(**
The To_json.t<'a> typeclass, instances for primitive types, and instance
builders for arbitrary (product) data types.
*)
module To_json =
  (**
  Essentially this is just a wrapper for a function which can convert a
  given type 'a to a JSON string. A good way to think of it is also as a
  proposition that a type 'a can be converted into a JSON string. The
  typeclass instances of the various types are proofs that those types
  can be converted into JSON strings.
  *)
  type t<'a>
  type key_value<'a> = KeyValuePair<string, 'a>

  (* JSON converter instances for primitive types. *)

  val int : t<int>
  val string : t<string>
  val float : t<float>
  val double : t<double>
  val char : t<char>
  val bool : t<bool>
  val option : t<'a> -> t<option<'a>>
  val array : t<'a> -> t<array<'a>>

  (*
  The following are used to create JSON converter instances that
  construct valid JSON objects out of at least one key-value pair. They
  work by extracting (or converting) the input value (of type 'a) into
  one or more output values which we already know can be converted. This
  way we can convert recursively into an arbitrary depth.
  *)

  val object1 : ('a -> key_value<'b1>) * t<'b1> -> t<'a>
  val object2 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    t<'a>

  val object3 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    t<'a>

  val object4 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    ('a -> key_value<'b4>) * t<'b4> ->
    t<'a>

  val object5 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    ('a -> key_value<'b4>) * t<'b4> ->
    ('a -> key_value<'b5>) * t<'b5> ->
    t<'a>

  val object6 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    ('a -> key_value<'b4>) * t<'b4> ->
    ('a -> key_value<'b5>) * t<'b5> ->
    ('a -> key_value<'b6>) * t<'b6> ->
    t<'a>

  val object7 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    ('a -> key_value<'b4>) * t<'b4> ->
    ('a -> key_value<'b5>) * t<'b5> ->
    ('a -> key_value<'b6>) * t<'b6> ->
    ('a -> key_value<'b7>) * t<'b7> ->
    t<'a>

  val object8 :
    ('a -> key_value<'b1>) * t<'b1> ->
    ('a -> key_value<'b2>) * t<'b2> ->
    ('a -> key_value<'b3>) * t<'b3> ->
    ('a -> key_value<'b4>) * t<'b4> ->
    ('a -> key_value<'b5>) * t<'b5> ->
    ('a -> key_value<'b6>) * t<'b6> ->
    ('a -> key_value<'b7>) * t<'b7> ->
    ('a -> key_value<'b8>) * t<'b8> ->
    t<'a>

  (** Operations meant to be imported directly into client code. *)
  module Ops =
    (**
    Returns a key-value pair of the given name and value. This is a
    helper function to make creating JSON instances easier.
    *)
    val key : name:string -> value:'a -> key_value<'a>

    (**
    Returns a JSON converter function from some type 'a to a string.
    *)
    val to_json : t<'a> -> ('a -> string)

    (**
    Returns a JSON converter instance for an arbitrary type 'a given a
    function that can convert an 'a value into a JSON string.
    *)
    val make_to_json : ('a -> string) -> t<'a>
