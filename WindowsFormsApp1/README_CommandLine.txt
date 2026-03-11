==========================================
MarkingMateMulti - Command Line Mode
==========================================

QUICK START
==========================================

1. Build the project:
   - Option A: Open in Visual Studio and build (x64 platform)
   - Option B: Run Build.bat

2. Test GUI mode first:
   - Double-click WindowsFormsApp1.exe
   - Click "Initialize"
   - Test drawing and marking manually

3. Test command line mode:
   - Run TestCommandLine_Debug.bat
   - Or manually: WindowsFormsApp1.exe --board 0 --line 0,0,10,10 --mark


AVAILABLE BATCH FILES
==========================================

Build.bat
  - Automatically finds MSBuild and compiles the project
  - Output: bin\x64\Debug\WindowsFormsApp1.exe

TestCommandLine_Debug.bat
  - Test command line mode with diagnostic output
  - Tests both drawing and marking
  - Shows exit codes and troubleshooting tips

CommandLineExamples.bat
  - 5 usage examples
  - Demonstrates different features


COMMAND LINE USAGE
==========================================

Basic syntax:
  WindowsFormsApp1.exe [options]

Options:
  --help, -h, /?              Show help message
  --board <0-3>, -b <0-3>     Specify board number (default: 0)
  --config <path>             Config path (default: /cfg_config_MM1)
  --line <x1,y1,x2,y2>        Add single line segment
  --lines <list>              Add multiple lines (semicolon separated)
  --mark, -m                  Execute marking

Examples:
  WindowsFormsApp1.exe --help
  WindowsFormsApp1.exe --board 0 --line 0,0,50,50
  WindowsFormsApp1.exe --board 0 --line 0,0,50,50 --mark
  WindowsFormsApp1.exe --board 1 --lines "0,0,50,50;10,10,40,40" --mark


EXIT CODES
==========================================

0 - Success
1 - Initialization failed
2 - Drawing failed
3 - Marking failed
4 - Parameter error


TROUBLESHOOTING
==========================================

No laser output but Exit Code = 0:
----------------------------------
1. Rebuild the project (code was updated to fix timing issues)
2. Test with GUI mode - if GUI works, command line should work
3. Try longer lines: --line -40,-40,40,40 --mark
4. Use DebugView to see detailed output:
   - Download: https://docs.microsoft.com/sysinternals/downloads/debugview
   - Run DebugView before testing
   - Check Debug.WriteLine messages

Exit Code 1 (Initialization failed):
------------------------------------
- Check MarkingMate SDK installation
- Verify hardware connection
- Test with GUI mode first

Exit Code 2 (Drawing failed):
-----------------------------
- Check coordinate format (x1,y1,x2,y2)
- Ensure coordinates are in range (-75 to +75)

Exit Code 3 (Marking failed):
-----------------------------
- Use GUI mode "Test Connection" to check hardware
- Verify control card IP settings
- Check laser power settings in MarkingMate software


COORDINATE SYSTEM
==========================================

Origin: (0, 0) at workspace center
X-axis: Right is positive
Y-axis: Down is positive
Unit: Millimeters (mm)
Valid range: -75 to +75
Workspace size: 150 x 150 mm


DEBUG OUTPUT
==========================================

To see detailed execution logs:
1. Download DebugView from Microsoft Sysinternals
2. Run DebugView (as administrator)
3. Execute command line test
4. Check DebugView for messages like:
   - "Line drawn: (0, 0) -> (50, 50)"
   - "Marking started, waiting for completion..."
   - "Marking in progress... (1000ms)"
   - "Marking completed (elapsed XXXms)"


FILES MODIFIED
==========================================

New files:
- CommandLineArgs.cs         (Command line parser)
- Build.bat                   (Build script)
- TestCommandLine_Debug.bat   (Test script)
- CommandLineExamples.bat     (Examples)
- README_CommandLine.txt      (This file)

Modified files:
- Program.cs                  (Added parameter handling)
- Form1.cs                    (Added auto mode)
- MarkingMateMulti.csproj     (Added CommandLineArgs.cs)


NOTES
==========================================

- GUI mode is fully preserved - no parameters = GUI mode
- Command line mode creates hidden window (required for ActiveX)
- Each execution can only operate on one board
- For multiple boards, call the program multiple times
- Platform must be x64
- Requires .NET Framework 4.7.2
- Requires MarkingMate SDK installed


SUPPORT
==========================================

For detailed documentation, see:
- Command Line User Guide (Traditional Chinese)
- Implementation Details (Traditional Chinese)
- Troubleshooting Guide (Traditional Chinese)

==========================================
