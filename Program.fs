open System
open System.IO
open System.Windows.Forms
open Operators
open System.Drawing

[<EntryPoint>]
[<STAThread>]
let main argv = 
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault false

    //MUTABLE VARIABLES
    let mutable fileName = ""
    let mutable savedFile = false
    let mutable text = ""
    
    use form = new Form()
    let dynamicPanel = new Panel()
    form.Visible <- true
    form.Text <- "F# Forms"
    form.Height <- 530
    form.Width <- 650

    let box = new RichTextBox()
    box.Multiline <- true
    box.AcceptsTab <- true
    box.Height <- form.Height
    box.Width <- form.Width
    box.WordWrap <- false
    box.Location <- System.Drawing.Point(0, 30)

    form.AutoSize <- true
    form.AutoSizeMode <- AutoSizeMode.GrowOnly

    dynamicPanel.AutoSize <- true
    dynamicPanel.AutoSizeMode <- AutoSizeMode.GrowOnly
    dynamicPanel.Dock <- DockStyle.Fill

    let menuBar = new MenuStrip()

    //Creating the main menu items
    let fileM = new ToolStripMenuItem("File")
    let editM = new ToolStripMenuItem("Edit")
    let formM = new ToolStripMenuItem("Format")
    let helpM = new ToolStripMenuItem("Help")
    menuBar.Items.Add(fileM) |> ignore
    menuBar.Items.Add(editM) |> ignore
    menuBar.Items.Add(formM) |> ignore
    menuBar.Items.Add(helpM) |> ignore

    //Filling the File menu
    let newFile = new ToolStripMenuItem("New")
    let openFile = new ToolStripMenuItem("Open...")
    let saveFile = new ToolStripMenuItem("Save")
    let saveAsFile = new ToolStripMenuItem("Save As...")
    let exitFile = new ToolStripMenuItem("Exit")
    fileM.DropDownItems.Add(newFile) |> ignore
    fileM.DropDownItems.Add(openFile) |> ignore
    fileM.DropDownItems.Add(saveFile) |> ignore
    fileM.DropDownItems.Add(saveAsFile) |> ignore
    fileM.DropDownItems.Add("-") |> ignore
    fileM.DropDownItems.Add(exitFile) |> ignore

    //Filling the Edit menu 
    let undoEdit = new ToolStripMenuItem("Undo")
    let redoEdit = new ToolStripMenuItem("Redo")
    let cutEdit = new ToolStripMenuItem("Cut")
    let copyEdit = new ToolStripMenuItem("Copy")
    let pasteEdit = new ToolStripMenuItem("Paste")
    let deleteEdit = new ToolStripMenuItem("Delete")
    let selectAllEdit = new ToolStripMenuItem("Select All")
    let timeDateEdit = new ToolStripMenuItem("Time/Date")
    editM.DropDownItems.Add(undoEdit) |> ignore
    editM.DropDownItems.Add(redoEdit) |> ignore
    editM.DropDownItems.Add("-") |> ignore
    editM.DropDownItems.Add(cutEdit) |> ignore
    editM.DropDownItems.Add(copyEdit) |> ignore
    editM.DropDownItems.Add(pasteEdit) |> ignore
    editM.DropDownItems.Add(deleteEdit) |> ignore
    editM.DropDownItems.Add("-") |> ignore
    editM.DropDownItems.Add(selectAllEdit) |> ignore
    editM.DropDownItems.Add(timeDateEdit) |> ignore
    
    //Filling the Format menu
    let wordWrapForm = new ToolStripMenuItem("Word Wrap")
    let fontForm = new ToolStripMenuItem("Font...")
    let colorForm = new ToolStripMenuItem("Font Color")
    formM.DropDownItems.Add(wordWrapForm) |> ignore
    formM.DropDownItems.Add(fontForm) |> ignore
    formM.DropDownItems.Add(colorForm) |> ignore

    //Filling the Font... menu inside Format
    let regularFont = new ToolStripMenuItem("Regular")
    let boldFont = new ToolStripMenuItem("Bold")
    let italicFont = new ToolStripMenuItem("Italic")
    let underlineFont = new ToolStripMenuItem("Underline")
    let strikeoutFont = new ToolStripMenuItem("Strikeout")
    fontForm.DropDownItems.Add(regularFont) |> ignore
    fontForm.DropDownItems.Add(boldFont) |> ignore
    fontForm.DropDownItems.Add(italicFont) |> ignore
    fontForm.DropDownItems.Add(underlineFont) |> ignore
    fontForm.DropDownItems.Add(strikeoutFont) |> ignore
    
    //Filling the Font Color menu inside Format
    let blackColor = new ToolStripMenuItem("Black")
    let redColor = new ToolStripMenuItem("Red")
    let greenColor = new ToolStripMenuItem("Green")
    let blueColor = new ToolStripMenuItem("Blue")
    let yellowColor = new ToolStripMenuItem("Yellow")
    let whiteColor = new ToolStripMenuItem("White")
    colorForm.DropDownItems.Add(blackColor) |> ignore
    colorForm.DropDownItems.Add(redColor) |> ignore
    colorForm.DropDownItems.Add(greenColor) |> ignore
    colorForm.DropDownItems.Add(blueColor) |> ignore
    colorForm.DropDownItems.Add(yellowColor) |> ignore
    colorForm.DropDownItems.Add(whiteColor) |> ignore

    //Filling the Help menu
    let aboutHelp = new ToolStripMenuItem("About")
    helpM.DropDownItems.Add(aboutHelp) |> ignore

    //EVENT HANDLERS
    //ALSO USES LAMBDA FUNCTIONS
    saveAsFile.Click.Add(fun saveAsFile_Click -> let saveFileDlg = new SaveFileDialog()
                                                 saveFileDlg.Filter <- "Text File|*.txt"
                                                 saveFileDlg.Title <- "Save a Text File"
                                                 if(saveFileDlg.ShowDialog(form).Equals(DialogResult.OK)) then
                                                    fileName <- saveFileDlg.FileName
                                                    text <- box.Text.ToString()
                                                    if(not(File.Exists(fileName))) then
                                                        File.WriteAllText(fileName, text)
                                                    savedFile <- true
    )

    saveFile.Click.Add(fun saveFile_Click -> if(not(savedFile)) then
                                                saveAsFile.PerformClick()
                                             else
                                                text <- box.Text.ToString()
                                                if(not(File.Exists(fileName))) then
                                                    File.WriteAllText(fileName, text)
    )

    openFile.Click.Add(fun openFile_Click -> savedFile <- true
                                             let openFileDlg = new OpenFileDialog()
                                             openFileDlg.Filter <- "Text File|*.txt"
                                             openFileDlg.Title <- "Open a Text File"
                                             if(openFileDlg.ShowDialog() = DialogResult.OK) then
                                                fileName <- openFileDlg.FileName
                                                use stream = new StreamReader(fileName)
                                                box.Clear()
                                                text <- ""
                                                let mutable valid = true
                                                while(valid) do
                                                    let line = stream.ReadLine()
                                                    if(line = null) then
                                                        valid <- false
                                                    else
                                                        text <- text + line + "\n" 
                                                box.Text <- text
    )

    newFile.Click.Add(fun newFile_Click -> if(not(savedFile)) then
                                               let result = MessageBox.Show("Do you want to save first?",
                                                                            "File not saved!", 
                                                                            MessageBoxButtons.YesNoCancel, 
                                                                            MessageBoxIcon.Exclamation)
                                               if(result = DialogResult.Yes) then
                                                    saveFile.PerformClick()
                                                    newFile.PerformClick()
                                               else if(result = DialogResult.No) then
                                                    text <- ""
                                                    box.Clear()
                                                    box.SelectionColor <- Color.Black
                                                    box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Regular)
                                                    box.WordWrap <- false
                                           else 
                                               savedFile <- false
                                               text <- ""
                                               box.Clear()
                                               box.SelectionColor <- Color.Black
                                               box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Regular)
                                               box.WordWrap <- false
              
    )

    selectAllEdit.Click.Add(fun selectAll_Click -> box.SelectAll())

    exitFile.Click.Add(fun exit_Click -> form.Close())

    undoEdit.Click.Add(fun undo_Click -> box.Undo())

    redoEdit.Click.Add(fun undo_Click -> box.Redo())

    cutEdit.Click.Add(fun cut_Click -> box.Cut())

    copyEdit.Click.Add(fun copy_Click -> box.Copy())

    pasteEdit.Click.Add(fun paste_Click -> box.Paste())

    deleteEdit.Click.Add(fun del_Click -> box.SelectedText <- "")

    wordWrapForm.Click.Add(fun wrap_Click -> box.WordWrap <- not(box.WordWrap))

    timeDateEdit.Click.Add(fun timeDate_Click -> let dt = DateTime.Now
                                                 let dateString = dt.Month.ToString() + "/" + dt.Day.ToString() + "/" + dt.Year.ToString()
                                                 let timeString = dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString()
                                                 box.AppendText(dateString + " " + timeString))

    aboutHelp.Click.Add(fun about_Click -> MessageBox.Show("Program written by Sebastian Hernandez and Jesus Leon\nc. 2019\nCS 4080 Programming Concepts",
                                                           "F# Notepad", 
                                                           MessageBoxButtons.OK, 
                                                           MessageBoxIcon.Information) |> ignore)

    //Event Handlers for Font
    //METHODS
    let fontChangeRegular _ =
        box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Regular)

    regularFont.Click.Add(fontChangeRegular)

    let fontChangeBold _ =
        box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Bold)

    boldFont.Click.Add(fontChangeBold)

    let fontChangeItalic _ =
        box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Italic)

    italicFont.Click.Add(fontChangeItalic)

    let fontChangeStrikeout _ =
        box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Strikeout)

    strikeoutFont.Click.Add(fontChangeStrikeout)

    let fontChangeUnderline _ =
        box.SelectionFont <- new Font(box.SelectionFont.FontFamily, box.SelectionFont.Size, FontStyle.Underline)

    underlineFont.Click.Add(fontChangeUnderline)
    
    //Event Handlers for Font Color
    let colorChangeRed _ =
        box.SelectionColor <- Color.Red

    redColor.Click.Add(colorChangeRed)


    let colorChangeBlack _ =
        box.SelectionColor <- Color.Black

    blackColor.Click.Add(colorChangeBlack)

    let colorChangeGreen _ =
        box.SelectionColor <- Color.Green

    greenColor.Click.Add(colorChangeGreen)

    let colorChangeBlue _ =
        box.SelectionColor <- Color.Blue

    blueColor.Click.Add(colorChangeBlue)

    let colorChangeYellow _ =
        box.SelectionColor <- Color.Yellow

    yellowColor.Click.Add(colorChangeYellow)

    let colorChangeWhite _ =
        box.SelectionColor <- Color.White

    whiteColor.Click.Add(colorChangeWhite)

    menuBar.Dock = DockStyle.Top |> ignore
    
    //Adding auto resize options and adding to form
    dynamicPanel.Controls.Add(box)
    dynamicPanel.Controls.Add(menuBar)
    menuBar.Dock <- DockStyle.Top
    box.Dock <- DockStyle.Fill
    form.Controls.Add(dynamicPanel)

    Application.Run(form)
    0
