#Changelog

24-01-2021 - Updated version 0.5.0.3

-> Completed implementation of struct in language.
-> Can define and execute structs, can assign to struct fields.
-> Updated Editor to make use of new/modified ToString() overrides on various ASTs.
-> Deployed new release package.
-> Bugfix assignment of arrays and records. They are now properly copied/cloned instead of referenced on assignment.

22-01-2021 - Updated language.

-> Parsing for struct definition implemented.
-> Can assign struct to other variables to create a new named struct.

20-01-2021 - Updated language project to Version 0.5.0.2

-> Added this changelog to the solution.

-> Removed version numbers from readme.md

-> Removed Libraries dependancies where possible.
   Ast_Call uses Libraries to search for a procedure/function to execute.
   Parser instantiates Ast_Call. Both Ast_Call and Parser have Libraries in the constructor argument list.

-> Removed duplicate operand precedence table, constants and operands readonly static fields from Parser.
   These already existed in Ast_Expression.

-> Updated Constants in Ast_Expression to take into account params.

Information:
   Params is currently only implemented for use in the "console" procedure in SystemLibrary.


19-01-2021 - Version 0.5.0.1

Initial release on GitHub.
