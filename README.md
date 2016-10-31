# F# Typeclasses: a Dictionary Encoding Approach

A lot of people have tried to encode Haskell-style typeclasses or
ML-style modules and functors in F#. So far there have been two major
stumbling blocks: (1) F#'s lack of higher-kinded types, and (2) F#'s
lack of a compile-time mechanism for implicitly selecting
implementations based on types.

The state of the art in F# seems to be
https://github.com/palladin/Higher , a library for higher-kinded
programming in F#. The problem with Higher and most other F# typeclass
efforts is that they make heavy use of object-oriented techniques,
which--although they work--are clunky and difficult to implement.
Writing new instances--the very thing that gives typeclasses their
extensibility--should be a piece of cake.

This library, `fsharp-typeclasses`, aims to make that possible by
borrowing ideas heavily from both Haskell and Scala.

## Approach

The basic idea is to encode a typeclass as a record type holding the
methods (the typeclass operations), and encode instances as record
values. This is pretty much how Haskell implements it behind the scenes.
However, we take some code organisation steps to make things easier on
ourselves.

### Module Structure

We structure a typeclass (e.g. `Functor`) as a `module Functor` in a
file `functor.fs` with a corresponding signature file `functor.fsi`. We
use the signature file heavily in our approach, to help us constrain our
types but also keep the implementation clean and clutter-free.

In the `Functor` module we have a record type `t` which has a field
corresponding to the `Functor` map method, with an appropriate function
type.

Next, we have the various instances which are appropriate to declare in
the typeclass module, as they're based on standard library types like
`option`, `list` and `array`.

Next, we have a nested module `Ops` which exposes the typeclass's
methods in an easy-to-use way given any typeclass instance. The
intention is that the user will open this module to get all the
typeclass operations; they won't actually have to open the actual
top-level typeclass module itself. Of course, they'll need to pass in
instances, but they can access the instances safely qualified by the
module name, e.g.

```fsharp
open Functor.Ops

let list = map Functor.list fst [1, 2; 3, 4]
//  list = [1; 3]
```

Finally, we (optionally) have another nested module `Laws` which encodes
the laws the typeclass is expected to obey in the form of functions
which take the relevant instances and any other inputs they need and
output a `bool` indicating whether the law is obeyed or not for those
instances and values. This practice is also used quite often in Haskell
and Scala to ship typeclasses with their expected invariant behaviours
in the code itself.

### Signature Files

As mentioned, we make heavy use of signature files to assign exact
typing information to all our functions and values. The feedback loop
between implementation and signature files helps to derive
higher-quality code.

### Higher-Kinded Types

We skip over the problem of the lack of higher-kinded types by just
passing in _fully-applied_ types as type parameters. This means that
instead of having a `Functor.t<'f>`, we have a `Functor.t<'a, 'b, 'fa,
'fb>`. Now, admittedly, there's no guarantee with these type parameters
that they'll actually obey the functor requirements, e.g. if `'fa` is a
`list<'a>` then `'fb` must be a `list<'b>`. There's nothing enforcing
this at compile time. But in my opinion we partially make up for that by
shipping functor laws right beside the functor definition; once we
implement property-based testing of the laws, they will be almost as
solid as in languages with higher-kinded types.

## Explicit Dictionary Passing

For now, we are forced to pass in typeclass instances explicitly as
shown above. F# does not have a general-purpose implicit resolution
technique available. However, we are no worse off here than with ML
modules and having work with them explicitly. Also, there may be some
way to leverage F# code quotations to simulate implicit resolution.

## Exploration with JSON Encoding

So far the most comprehensive exploration of this typeclass technique is
in the `To_json` module. We provide instances for simple types and
combinators to derive instances, ranging in complexity from simple
(deriving a JSON converter of an array of something given a converter
instance for that thing) to complex (deriving a converter instances for
an arbitrary product type given multiple converter instances for its
component types).

Note in particular the full suite of unit tests of the JSON instances in
the `FsharpTypeclassesTest.To_json_test` module. These tests show a good
cross-section of usages, from building instances out of simpler
instances (much like ML functors, actually) to using the instances for
JSON conversion.
