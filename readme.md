Eveonline console app which reads logs, this is started because other systems are not open sourced.
Foxjazz will guide and manage updates to this project

Use this for replacing bad characters
var replacements = new[] { "@", "*", "\"", "&", "^", "%", "$", "#", "!", "=", "(", ")", "[", "]", "{", "}" };
var output = new StringBuilder(Input);
foreach (var r in replacements)
    output.Replace(r, string.Empty);
