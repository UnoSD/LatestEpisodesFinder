module Parser

open CommandLine
open System.Globalization
open CommandLineOptions
open System

type CommandLineOption = 
      Find of FindOptions
    | Scan of ScanOptions
    | Error of string list

let parseArguments args =
    let settings (settings : ParserSettings) =
        settings.ParsingCulture <- CultureInfo.CurrentCulture;
        settings.IgnoreUnknownArguments <- false

    let settingsAction =
        Action<ParserSettings>(settings)

    use parser =
        new Parser(settingsAction) 

    let parseResult =
        args |> parser.ParseArguments<FindOptions, ScanOptions>

    let toOption (result : obj) =
        match result with
        | :? FindOptions as o -> Find o
        | :? ScanOptions as o -> Scan o
        | _                   -> Error [ "Invalid verb" ]
        
    let toError (notParsed : NotParsed<obj>) =
        notParsed.Errors |>
        Seq.map (fun e -> e.ToString()) |>
        Seq.toList |>
        Error

    match parseResult with
    | :? Parsed<obj>    as parsed    -> parsed.Value |> toOption
    | :? NotParsed<obj> as notParsed -> notParsed    |> toError
    | _                              -> [ "Invalid parser result." ] |> Error


type FindVerbOption =
    | NoArguments
    | What of string
    | Error of string list

let parseFindArguments args =
    let settings (settings : ParserSettings) =
        settings.ParsingCulture <- CultureInfo.CurrentCulture;
        settings.IgnoreUnknownArguments <- false

    let settingsAction =
        Action<ParserSettings>(settings)

    use parser =
        new Parser(settingsAction) 

    let parseResult =
        args |> parser.ParseArguments<FindVerbOptions>

    let toFindOption (o : FindVerbOptions) =
        match o.what with
        | ""     -> NoArguments
        | "all"  -> NoArguments
        | "film" -> What "film"
        | _      -> Error [ "Boh" ]

    let toOption (result : obj) =
        match result with
        | :? FindVerbOptions as o -> toFindOption o
        | _                       -> Error [ "Invalid verb" ]
        
    let toError (notParsed : NotParsed<FindVerbOptions>) =
        notParsed.Errors |>
        Seq.map (fun e -> e.ToString()) |>
        Seq.toList |>
        Error

    match parseResult with
    | :? Parsed<FindVerbOptions>    as parsed    -> parsed.Value |> toOption
    | :? NotParsed<FindVerbOptions> as notParsed -> notParsed    |> toError
    | _                                          -> [ "Invalid parser result." ] |> Error