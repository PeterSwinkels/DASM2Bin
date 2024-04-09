'This module's imports and settings.
Option Compare Binary
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Convert
Imports System.Environment
Imports System.IO
Imports System.Linq

'This module contains this program's core procedures.
Public Module CoreModule

   'This procedure is executed when this program is started.
   Public Sub Main()
      Try
         Dim Binary As New List(Of Byte)
         Dim Code As List(Of String)
         Dim InputPath As String = Nothing
         Dim OutputPath As String = Nothing

         If GetCommandLineArgs.Count = 2 Then
            InputPath = GetCommandLineArgs.Last()
            Code = New List(Of String)(From Line In File.ReadAllLines(InputPath) Select Line.Trim({ToChar(9), " "c}))
            Code = New List(Of String)(From Line In Code Where Not Line = Nothing AndAlso Not Line.StartsWith(";"))
            Code = New List(Of String)(From Line In Code Select Line.Substring(Line.IndexOf(" "c)).Trim())
            Code = New List(Of String)(From Line In Code Select Line.Substring(0, Line.IndexOf(" "c)).Trim())

            For Each Line As String In Code
               Console.WriteLine(Line)
               For Position As Integer = 0 To Line.Length - 1 Step 2
                  Binary.Add(ToByte(Line.Substring(Position, 2), fromBase:=16))
               Next Position
            Next Line

            OutputPath = Path.Combine(Path.GetDirectoryName(InputPath), $"{Path.GetFileNameWithoutExtension(InputPath)}.bin")
            File.WriteAllBytes(OutputPath, Binary.ToArray())
            Console.WriteLine($"Saved as: {OutputPath}")
         Else
            Console.WriteLine(ProgramInformation())
            Console.WriteLine()
            Console.WriteLine($"Usage: {My.Application.Info.AssemblyName} INPUT_FILE")
         End If
      Catch ExceptionO As Exception
         DisplayException(ExceptionO)
      End Try
   End Sub

   'This procedure displays any exceptions that occur.
   Private Sub DisplayException(ExceptionO As Exception)
      Try
         Console.WriteLine($"ERROR: {ExceptionO.Message}")
      Catch
         [Exit](0)
      End Try
   End Sub

   'This procedure returns this program's information.
   Private Function ProgramInformation() As String
      Try
         With My.Application.Info
            Return $"{ .Title} v{ .Version} - by: { .CompanyName}"
         End With
      Catch ExceptionO As Exception
         DisplayException(ExceptionO)
      End Try

      Return Nothing
   End Function
End Module
