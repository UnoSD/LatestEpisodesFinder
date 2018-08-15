open CommandLine
open System
open System.Globalization

[<Verb("find", HelpText = "Find media in the database.")>]
type FindOptions = 
    {
        [<Option(HelpText = "Input the name of the media to find.", Default="*")>]
        searchTerm : string;
    }

[<Verb("list", HelpText = "List all media in the database.")>]
type ScanOptions = 
    {
        [<Option>]
        path : string;
    }

type ResultBuilder() =
    member __.Bind(x, (f : 'c -> Result<'f,'d>)) =
        match x with
        | Ok x    -> f(x)
        | error   -> error

    member __.Return(x) =
        Ok x

let result = new ResultBuilder()

let parseArguments args =
    let settings (settings : ParserSettings) =
        settings.ParsingCulture <- CultureInfo.CurrentCulture;
        settings.IgnoreUnknownArguments <- false

    let settingsAction =
        Action<ParserSettings>(settings)

    use parser =
        new Parser(settingsAction) 

    args |> parser.ParseArguments<FindOptions, ScanOptions>

[<EntryPoint>]
let main args =
    let result =
        result {
                   let! parsed =
                       match args |> Array.toList with
                       | "find"::tail -> Ok (parseArguments tail)
                       | _            -> Result.Error [ "Unknown verb." ]

                   let! parsed =
                       match parsed with
                       | :? Parsed<obj>    as parsed    -> Ok            parsed.Value
                       | :? NotParsed<obj> as notParsed -> Result.Error  (notParsed.Errors |> 
                                                                          Seq.map (fun e -> e.ToString()) |> 
                                                                          Seq.toList)
                       | _                              -> Result.Error  [ "Invalid parser result." ]

                   let unit =
                       match parsed with
                       | :? FindOptions as o -> printf "find %A" o.searchTerm
                       | :? ScanOptions as o -> printf "list %A" o.path
                       | _                   -> printf "bad"
                   
                   return 0
               }
    
    match result with
    | Success x -> x
    | Failure e -> printf "%A" e
                   1