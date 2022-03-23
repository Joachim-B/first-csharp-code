public static class Global { //Les valeurs ci-dessous peuvent être modifiée (sauf la List, bien entendu)
    public static List<string> contents = new List<string>{};
    public const int nbColumn = 3;
    public const int spaceBetweenBox = 6;
    public const int boxSize = 40;
    public const int boxPadding = 5;
    public const int contentSize = boxSize - (boxPadding*2);
}

class Program {
    static void Main(string[] args) {

        LoadContent();

        int countBox = Global.contents.Count;

        if(countBox == 0) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Fichier \"content.txt\" introuvable ou sans contenu, veuillez créer ce fichier dans le même dossier que Program.cs.");
            Console.ForegroundColor = ConsoleColor.Gray;
            return;
        }

        int[] idColumn = new int[Global.nbColumn];

        for(int i = 0; i < Global.nbColumn;i++) {
            idColumn[i] = i;
        }

        string[] textColumn = new string[Global.nbColumn];
        
        int nbLines = DefineNbLines(countBox);

        int currentLine = 0;


        while(currentLine < nbLines) {

            bool[] columnDone = new bool[Global.nbColumn];

            int[] columnState = new int[Global.nbColumn];
            // 0 = top line ; 1/3 = top padding ; 2 = show ID ; 4 = text line ; 5 = bottom padding ; 6 = bottom line

            for(int i = 0;i<Global.nbColumn;i++) {

                columnDone[i] = false;
                columnState[i] = 0;

                if(Global.contents.ElementAtOrDefault(idColumn[i]) != null) {
                    textColumn[i] = Global.contents[idColumn[i]];
                } else {
                    columnDone[i] = true;
                }
            }
            //insertion du texte à écrire dans la colonne à chaque textColumn[i], + s'il n'y a pas de texte à écrire, on marque la colonne comme étant terminée d'être écrite.

            bool allColumnsDone = true;

            foreach(bool check in columnDone) {
                if(check == false) {
                    allColumnsDone = false;
                    break;
                }
            }
            //check si au moins une colonne a du contenu à écrire

            while (allColumnsDone == false) {

                for(int i = 0; i < Global.nbColumn; i++) {

                    if(!columnDone[i]) {

                        switch (columnState[i])
                        {
                            case < 4 :
                                TopLine(ref columnState[i], idColumn[i]);
                                break;
                            
                            case 4 :
                                MiddleLine(ref textColumn[i], ref columnState[i]);
                                break;
                            
                            case > 4 :
                                BottomLine(ref columnState[i]);
                                if(columnState[i] == 6) {
                                    columnDone[i] = true;
                                }
                                break;

                            default:;
                        }
                    
                    } else {
                        if(i != Global.nbColumn-1) {
                            for(int j = 0; j < Global.spaceBetweenBox+Global.boxSize+2;j++) {
                            Console.Write(' ');
                            }
                        }
                    }
                }

                Console.Write("\n");

                allColumnsDone = true;
                foreach(bool check in columnDone) {
                    if(check == false) {
                        allColumnsDone = false;
                        break;
                    }
                }
                //check si des colonnes ont encore du contenu à écrire. Dans ce cas, allColumnsDone reste à false
            }

            Console.Write("\n");
            currentLine++;

            for(int i = 0; i<Global.nbColumn;i++) {
                idColumn[i] += Global.nbColumn;
            }

        }
    }


    static void LoadContent() {

        string? txtLine = "";

        try {
            using (StreamReader sr = new StreamReader("content.txt"))
            {
                while ((txtLine = sr.ReadLine()) != null)
                {
                    if(!String.IsNullOrWhiteSpace(txtLine)) {
                        Global.contents.Add(txtLine);
                    }
                }
            }
        }
        catch {};
    }

    static int DefineNbLines(int totalBox) {
        
        for(int i = 0; i<Global.nbColumn;i++) {
            if((totalBox+ i) % Global.nbColumn == 0) {
                return (totalBox+ i) / Global.nbColumn;
            }
        }
        return 0;
    }



    static void TopLine(ref int state, int id) {

        for (int i = 0; i< Global.spaceBetweenBox; i++) {
            Console.Write(' ');
        }


        if( state == 0) {

            Console.Write(' ');

            for(int i = 0; i< Global.boxSize; i++) {
                Console.Write('_');
            }

            Console.Write(' ');

            state = 1;

        } else if (state == 2) {

            Console.Write('|');

            for(int i = 0; i<Global.boxPadding;i++) {
                Console.Write(' ');
            }

            string showID = "ID : " + id;
            Console.Write(showID);

            for(int i = showID.Length+Global.boxPadding; i < Global.boxSize;i++) {
                Console.Write(' ');
            }

            Console.Write('|');

            state = 3;

        } else {

            Console.Write('|');
            
            for(int i = 0; i<Global.boxSize;i++) {
                Console.Write(' ');
            }

            Console.Write('|');

            if(state == 1) {
                state = 2;
            } else {
                state = 4;
            }
        }

    }

    static void MiddleLine(ref string text, ref int state) {
        for (int i = 0; i< Global.spaceBetweenBox; i++) {
            Console.Write(' ');
        }

        Console.Write('|');

        for (int i = 0; i<Global.boxPadding; i++) {
            Console.Write(' ');
        }

        ApplyText(ref text, ref state);

        for (int i = 0; i<Global.boxPadding; i++) {
            Console.Write(' ');
        }

        Console.Write('|');
    }


    static void BottomLine(ref int state) {

        char character = '_';
        state = 6;
        if(state == 4) {
            character = ' ';
            state = 5;
        }
        for (int i = 0; i< Global.spaceBetweenBox; i++) {
        Console.Write(' ');
        }

        Console.Write('|');
        
        for(int i = 0; i<Global.boxSize;i++) {
            Console.Write(character);
        }

        Console.Write('|');

    }



    static void EmptyLine(ref int state) {
        for (int i = 0;i<Global.contentSize;i++) {
            Console.Write(' ');
        }

        state = 5;
    }

    static void ApplyText(ref string text, ref int state) {

        int position = Global.contentSize;

        if(text.Length <= Global.contentSize) {
            Console.Write(text);

            position = text.Length;
            while(position < Global.contentSize) {
                Console.Write(' ');
                position++;
            }
            state = 5;

        } else {

            string newText = "";

            while(position > 0) {
                if(text[position] == ' ') {

                    Console.Write("{0}",text.Substring(0,position));
                    newText = text.Substring(position+1);

                    while(position < Global.contentSize) {
                        Console.Write(' ');
                        position++;
                    }
                    break;
                }
                position--;
            }

            if(position == 0) {
                Console.Write("{0}",text.Substring(0,Global.contentSize-1));
                Console.Write('-');
                newText = text.Substring(Global.contentSize-1);
            }
            text = newText;
        }

    }
}