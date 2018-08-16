module CommandLineOptions

open CommandLine

[<Verb("find", HelpText = "Find media in the database.")>]
type FindOptions = 
    {
        [<Option(HelpText = "Input the name of the media to find.", Default="*")>]
        searchTerm : string
    }

[<Verb("list", HelpText = "List all media in the database.")>]
type ScanOptions = 
    {
        [<Option>]
        path : string
    }