module ResultBuilder

type ResultBuilder() =
    member __.Bind(x, (f : 'c -> Result<'f,'d>)) =
        match x with
        | Ok x    -> f(x)
        | error   -> error

    member __.Return(x) =
        Ok x

let result = new ResultBuilder()