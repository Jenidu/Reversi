/* Library import */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

/* Grootte en startposities */
int[] Venster_Grootte = {760, 545}; /* Venster grootte */
int[] StartBM = {15, 130};  /* Start positie van bitmap, relatief tot de venster */
int[] BM_Grootte = {400, 400};  /* Bitmap grootte */

/* Tekst boxen grootte en posities */
int[] StartVensterTekst = {10, 15};
int[] TekstBox_PosVerschil = {0, 25};


Color[] steen_kleuren = {Color.Red, Color.Blue};  /* Steen kleuren */

/* Scherm venster */
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.Beige;
scherm.ClientSize = new Size(Venster_Grootte[0], Venster_Grootte[1]);

/* Labels */
Label hello_world = new Label();

/* Knoppen */
Button help_knop = new Button();

/* Bitmaps */
Bitmap plaatje = new Bitmap(BM_Grootte[0], BM_Grootte[1]);

/* Labels */
Label label_plaatje = new Label();

/* Dropdown */
ComboBox nieuwspel_opties = new ComboBox();

/* Spelsituatie */
int[,] game_state = new int[16, 16];  /* -1: buiten veld, 0: van niemand, 1: van speler 1, 2: van speler 2 */
int tile_size, tile_dim;  /* Grid properties */



opstartVenster();

nieuwspel_opties.SelectedIndexChanged += nieuwSpelOpties;  /* Dropdown veranderd */

help_knop.Click += helpLaden;  /* Er is op 'help' geklikt */

Application.Run(scherm);

void opstartVenster()
{
    /* Label voor de bitmap */
    scherm.Controls.Add(label_plaatje);
    label_plaatje.Location = new Point(StartBM[0], StartBM[1]);
    label_plaatje.Size = new Size(BM_Grootte[0], BM_Grootte[1]);
    label_plaatje.BackColor = Color.White;
    label_plaatje.Image = plaatje;

    /* Tekst toevoegen bij het input gedeelte */
    scherm.Controls.Add(hello_world);
    hello_world.Location = new Point(StartVensterTekst[0], StartVensterTekst[1]);
    hello_world.Size = new Size(80, 15);
    hello_world.Text = "midden x:";

    /* Knop aanmaken voor input gedeelte */
    scherm.Controls.Add(help_knop);
    help_knop.Location = new Point(250, 10);
    help_knop.Size = new Size(40, help_knop.Height);
    help_knop.Text = "help";

    /* Dropdown */
    scherm.Controls.Add(nieuwspel_opties);
    nieuwspel_opties.Location = new Point(120, 10);
    nieuwspel_opties.Text = "Nieuw spel";

    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 4x4 spel", "4x4"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 6x6 spel", "6x6"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 8x8 spel", "8x8"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 10x10 spel", "10x10"));
}

void nieuwSpelOpties(object o, EventArgs e)
{
    if (nieuwspel_opties.SelectedItem is DropdownItem selectedItem)  /* Een item is geselecteerd */
    {
        Console.WriteLine($"Selected type: {selectedItem.Type}, Text: {selectedItem.Text}");
        if (selectedItem.Type == "4x4")
            createNewBoard(4);
        else if (selectedItem.Type == "6x6")
            createNewBoard(6);
        else if (selectedItem.Type == "8x8")
            createNewBoard(8);
        else if (selectedItem.Type == "10x10")
            createNewBoard(10);
        else
            Console.WriteLine($"Incorrect item selected: type: {selectedItem.Type}, Text: {selectedItem.Text}");
    }
}

void createNewBoard(int size){

    tile_dim = size;  /* Nieuwe hoogte en breedte van het speelveld */

    for (int i = 0; i < 16; i++)  /* Reset board */
    {
        for (int j = 0; j < 16; j++)
            game_state[i, j] = -1;
    }



    updateStenenTeller();
}

void helpLaden(object o, EventArgs e){

}

void updateStenenTeller(){

    int[] stenen_teller = {0, 0};

    for (int i = 0; i < 16; i++)  /* Reset board */
    {
        for (int j = 0; j < 16; j++)
        {
            if (game_state[i, j] == 1)
                stenen_teller[0]++;
            else if (game_state[i, j] == 2)
                stenen_teller[1]++;
        }
    }
    //Diplay stenen

}

//MessageBox.Show($"Speler {} heeft gewonnen!");

public class DropdownItem
{
    public string Text { get; set; }
    public string Type { get; set; }

    public DropdownItem(string text, string type){
        Text = text;
        Type = type;
    }

    public override string ToString(){
        return Text;
    }
}
