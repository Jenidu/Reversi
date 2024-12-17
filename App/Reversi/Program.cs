/* Library import */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

/* Grootte en startposities */
int[] Venster_Grootte = {320, 510}; /* Venster grootte */

/* Tekst boxen grootte en posities */
int[] StartVensterTekst = {10, 15};
int[] TekstBox_PosVerschil = {0, 25};


Brush[] steen_kleuren = {Brushes.Blue, Brushes.Red};  /* Steen kleuren */

/* Scherm venster */
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.Beige;
scherm.ClientSize = new Size(Venster_Grootte[0], Venster_Grootte[1]);

/* Grid posities */
int[] grid_start = {10, 200};
int max_grid_size = 300;
int grid_size, tile_size, tile_dim;  /* Grid properties */

/* Knoppen */
Button help_knop = new Button();

/* Labels */
Label label_plaatje = new Label();

/* Dropdown */
ComboBox nieuwspel_opties = new ComboBox();

/* Spelsituatie */
int[,] game_state;  /* 0: van niemand, 1: van speler 1, 2: van speler 2 */

opstartVenster();
createNewBoard(6);

nieuwspel_opties.SelectedIndexChanged += nieuwSpelOpties;  /* Dropdown veranderd */

help_knop.Click += helpLaden;  /* Er is op 'help' geklikt */
scherm.Paint += teken;


Application.Run(scherm);

void opstartVenster()
{
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
        switch (selectedItem.Type)
        {
            case "4x4":
                createNewBoard(4);
                break;
            case "6x6":
                createNewBoard(6);
                break;
            case "8x8":
                createNewBoard(8);
                break;
            case "10x10":
                createNewBoard(10);
                break;
            default:
                Console.WriteLine($"Incorrect item selected: type: {selectedItem.Type}, Text: {selectedItem.Text}");
                break;
        }
    }
}

void createNewBoard(int size){

    tile_dim = size;  /* Nieuwe hoogte en breedte van het speelveld */

    tile_size = (int)((double)max_grid_size / size);
    grid_size = size * tile_size;  /* Fix rounding errors */

    game_state = new int [tile_dim,tile_dim];

    for (int i = 0; i < tile_dim; i++)  /* Reset board */
    {
        for (int j = 0; j < tile_dim; j++)
            game_state[i, j] = 0;
    }

    game_state[tile_dim/2 - 1, tile_dim/2 - 1] = 1;
    game_state[tile_dim/2, tile_dim/2] = 1;
    game_state[tile_dim/2, tile_dim/2 - 1] = 2;
    game_state[tile_dim/2 - 1, tile_dim/2] = 2;

    scherm.Invalidate();
}

void teken(object sender, PaintEventArgs pea){

    Graphics gr = pea.Graphics;

    int[] stenen_aantal = stenenTeller();

    // gr.FillEllipse(steen_kleuren[0], );
    // gr.DrawString(stenen_aantal[0] + " stenen", , steen_kleuren[0], 10, 10);
    // gr.FillEllipse(steen_kleuren[1], );
    // gr.DrawString(stenen_aantal[1] + " stenen", , steen_kleuren[1], 10, 10);


    int grid_rd_offset = (max_grid_size - grid_size) / 2;  /* Halve grid rounding offset */

    for (int x = 0; x <= grid_size; x += tile_size)  /* Teken grid verticaal */
    {
        int x_pos = x + grid_start[0] + grid_rd_offset; 
        gr.DrawLine(Pens.Black, x_pos, grid_start[1] + grid_rd_offset, x_pos, grid_start[1] + grid_rd_offset + grid_size);
    }

    for (int y = 0; y <= grid_size; y += tile_size)  /* Teken grid horizontaal */
    {
        int y_pos = y + grid_start[1] + grid_rd_offset;
        gr.DrawLine(Pens.Black, grid_start[0] + grid_rd_offset, y_pos, grid_start[0] + grid_rd_offset + grid_size, y_pos);
    }


    for (int x = 0; x < tile_dim; x++)  /* Teken circles */
    {
        for (int y = 0; y < tile_dim; y++)
        {
            if (game_state[x,y] == 1)

                gr.FillEllipse(steen_kleuren[0], grid_start[0] + tile_size*x, grid_start[1] + tile_size*y, tile_size, tile_size);

            else if (game_state[x,y] == 2)

                gr.FillEllipse(steen_kleuren[1], grid_start[0] + tile_size*x, grid_start[1] + tile_size*y, tile_size, tile_size);
        }
    }


}


void helpLaden(object o, EventArgs e){

}

int[] stenenTeller(){

    int[] stenen_teller = {0, 0};

    for (int i = 0; i < tile_dim; i++)  /* Reset board */
    {
        for (int j = 0; j < tile_dim; j++)
        {
            if (game_state[i, j] == 1)
                stenen_teller[0]++;
            else if (game_state[i, j] == 2)
                stenen_teller[1]++;
        }
    }
    
    return stenen_teller;
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
