open Parser
open CommandLineOptions

let find (f : FindOptions) =
    match parseFindArguments f.searchTerms with
    | NoArguments -> printf "find all"
    | What x      -> printf "find %A" x
    | Error x     -> printf "error %A" x

    printf "%A" f
    0

let scan f =
    printf "%A" f
    0

let error e =
    printf "%A" e
    1

[<EntryPoint>]
let main args =
    match parseArguments args with
    | Find  f -> find f
    | Scan  f -> scan f
    | CommandLineOption.Error e -> error e