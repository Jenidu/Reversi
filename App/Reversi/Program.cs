/* Library import */
using System;
using System.Drawing;
using System.Windows.Forms;

/* Grootte en startposities*/
int[] Venster_Grootte = { 760, 545 }; /* Venster grootte */
int[] StartBM = { 15, 130 };  /* Start positie van bitmap, relatief tot de venster */
int[] BM_Grootte = { 400, 400 };  /* Bitmap grootte */

/* Tekst boxen grootte en posities */
int[] StartVensterTekst = { 10, 15 };
int[] TekstBox_PosVerschil = { 0, 25 };

/* Scherm venster */
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.Beige;
scherm.ClientSize = new Size(Venster_Grootte[0], Venster_Grootte[1]);

/* Labels */
Label hello_world = new Label();

/* Knoppen */
Button ga_knop = new Button();

/* Bitmaps */
Bitmap plaatje = new Bitmap(BM_Grootte[0], BM_Grootte[1]);

/* Labels */
Label label_plaatje = new Label();

/* Dropdown */
ComboBox nieuwspel_opties = new ComboBox();

opstartVenster();

nieuwspel_opties.SelectedIndexChanged += nieuwSpelOpties;  /* Dropdown veranderd */

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
    scherm.Controls.Add(ga_knop);
    ga_knop.Location = new Point(225, 90);
    ga_knop.Size = new Size(40, 25);
    ga_knop.Text = "Ga!";

    /* Dropdown */
    scherm.Controls.Add(nieuwspel_opties);
    nieuwspel_opties.Location = new Point(50, 50);
    nieuwspel_opties.Text = "Nieuw spel";

    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 4x4 spel", 1));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 6x6 spel", 2));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 8x8 spel", 3));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 10x10 spel", 4));

}

void nieuwSpelOpties(object o, EventArgs e)
{
    Console.WriteLine($"You selected: {nieuwspel_opties.SelectedItem}");
}



public class DropdownItem
{
    public string Text { get; set; }
    public int Id { get; set; }

    public DropdownItem(string text, int id){
        Text = text;
        Id = id;
    }

    public override string ToString(){
        return Text;
    }
}
