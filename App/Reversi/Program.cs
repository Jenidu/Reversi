﻿/* Library import */
using System;
using System.Drawing;
using System.Windows.Forms;

/* Grootte en startposities */
int[] Venster_Grootte = {320, 510}; /* Venster grootte */

string[] speler_namen = {"blauw", "rood"};
Brush[] steen_kleuren = {Brushes.Blue, Brushes.Red};  /* Steen kleuren */

/* Scherm venster */
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.BackColor = Color.Beige;
scherm.ClientSize = new Size(Venster_Grootte[0], Venster_Grootte[1]);

/* Grid posities */
int[] grid_start = {10, 200};
int max_grid_size = 300;
int[] pos_voorbeeld_rondjes = {30, 40};
int r_voorbeeld_rondjes = 50;
int grid_size, tile_size, tile_dim;  /* Grid properties */

/* Knoppen */
Button help_knop = new Button();

/* Dropdown */
ComboBox nieuwspel_opties = new ComboBox();

/* Label */
Label aan_zet = new Label();

/* Spelsituatie */
int[,] game_state;  /* 0: van niemand, 1: van speler 1, 2: van speler 2 */
int current_player = 1;  /* Wie is er aan de beurt */
bool help = false, computer = false;

opstartVenster();
createNewBoard(6);

nieuwspel_opties.SelectedIndexChanged += nieuwSpelOpties;  /* Dropdown veranderd */

help_knop.Click += helpLaden;  /* Er is op 'help' geklikt */
scherm.Paint += teken;  /* Update graphics */
scherm.MouseClick += muisKlik;  /* Event handler voor in- en uitzoomen van de muis */

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
    nieuwspel_opties.Location = new Point(30, 10);
    nieuwspel_opties.Size = new Size(210, nieuwspel_opties.Height);
    nieuwspel_opties.Text = "Nieuw spel";

    /* Aan zet label */
    scherm.Controls.Add(aan_zet);
    aan_zet.Location = new Point(pos_voorbeeld_rondjes[0] + 5, pos_voorbeeld_rondjes[1] + r_voorbeeld_rondjes * 2 + 20);
    aan_zet.Size = new Size(100, 30);

    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 4x4 spel", "4x4"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 6x6 spel", "6x6"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 8x8 spel", "8x8"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 10x10 spel", "10x10"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 4x4 spel tegen computer", "4x4c"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 6x6 spel tegen computer", "6x6c"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 8x8 spel tegen computer", "8x8c"));
    nieuwspel_opties.Items.Add(new DropdownItem("Start nieuw 10x10 spel tegen computer", "10x10c"));
}

void nieuwSpelOpties(object o, EventArgs e)
{
    if (nieuwspel_opties.SelectedItem is DropdownItem selectedItem)  /* Een item is geselecteerd */
    {
        switch (selectedItem.Type)
        {
            case "4x4":
                createNewBoard(4);
                computer = false;
                break;
            case "6x6":
                createNewBoard(6);
                computer = false;
                break;
            case "8x8":
                createNewBoard(8);
                computer = false;
                break;
            case "10x10":
                createNewBoard(10);
                computer = false;
                break;
            case "4x4c":
                createNewBoard(4);
                computer = true;
                break;
            case "6x6c":
                createNewBoard(6);
                computer = true;
                break;
            case "8x8c":
                createNewBoard(8);
                computer = true;
                break;
            case "10x10c":
                createNewBoard(10);
                computer = true;
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

    current_player = 1;
    // help = false;

    scherm.Invalidate();
}

void teken(object sender, PaintEventArgs pea){

    Graphics gr = pea.Graphics;
    int x_pos, y_pos;

    int[] stenen_aantal = stenenTeller();

    gr.FillEllipse(steen_kleuren[0], pos_voorbeeld_rondjes[0], pos_voorbeeld_rondjes[1], r_voorbeeld_rondjes, r_voorbeeld_rondjes);
    gr.DrawString(stenen_aantal[0] + " stenen", new Font("Arial", 14), steen_kleuren[0], pos_voorbeeld_rondjes[0] + r_voorbeeld_rondjes + 10, pos_voorbeeld_rondjes[1] + 10);
    gr.FillEllipse(steen_kleuren[1], pos_voorbeeld_rondjes[0], pos_voorbeeld_rondjes[1] + r_voorbeeld_rondjes + 10, r_voorbeeld_rondjes, r_voorbeeld_rondjes);
    gr.DrawString(stenen_aantal[1] + " stenen", new Font("Arial", 14), steen_kleuren[1], pos_voorbeeld_rondjes[0] + r_voorbeeld_rondjes + 10, pos_voorbeeld_rondjes[1] + r_voorbeeld_rondjes + 20);

    aan_zet.Text = speler_namen[current_player-1] + " aan zet";

    int grid_rd_offset = (max_grid_size - grid_size) / 2;  /* Halve grid rounding offset */

    for (int x = 0; x <= grid_size; x += tile_size)  /* Teken grid verticaal */
    {
        x_pos = x + grid_start[0] + grid_rd_offset; 
        gr.DrawLine(Pens.Black, x_pos, grid_start[1] + grid_rd_offset, x_pos, grid_start[1] + grid_rd_offset + grid_size);
    }

    for (int y = 0; y <= grid_size; y += tile_size)  /* Teken grid horizontaal */
    {
        y_pos = y + grid_start[1] + grid_rd_offset;
        gr.DrawLine(Pens.Black, grid_start[0] + grid_rd_offset, y_pos, grid_start[0] + grid_rd_offset + grid_size, y_pos);
    }


    for (int x = 0; x < tile_dim; x++)  /* Teken circles in grid */
    {
        for (int y = 0; y < tile_dim; y++)
        {
            x_pos = grid_start[0] + tile_size*x + 2;
            y_pos = grid_start[1] + tile_size*y + 2;

            if (game_state[x,y] == 1)  /* Speler 1 heeft x,y */

                gr.FillEllipse(steen_kleuren[0], x_pos, y_pos, tile_size-4, tile_size-4);

            else if (game_state[x,y] == 2)  /* Speler 2 heeft x,y */

                gr.FillEllipse(steen_kleuren[1], x_pos, y_pos, tile_size-4, tile_size-4);
        }
    }

    if (help)  /* Hulp staat aan */
        tekenHelp(gr);
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

void tekenHelp(Graphics gr){

    bool[,] mogelijke_pos = new bool [tile_dim, tile_dim];
    int x_pos, y_pos;
    int ellip_size = tile_size / 2;

    for (int x = 0; x < tile_dim; x++)
    {
        for (int y = 0; y < tile_dim; y++)
        {
            if (game_state[x,y] == 0 && possiblePlacement(x, y))
            {
                x_pos = grid_start[0] + tile_size*x + tile_size/4;
                y_pos = grid_start[1] + tile_size*y + tile_size/4;

                gr.DrawEllipse(Pens.Black, x_pos, y_pos, ellip_size, ellip_size);
            }
        }
    }

}

void muisKlik(object o, MouseEventArgs ea){

    if (ea.X >= grid_start[0] && ea.Y >= grid_start[1])  /* Er wordt op het grid gedrukt */
    {
        int grid_x = (ea.X - grid_start[0]) / tile_size;
        int grid_y = (ea.Y - grid_start[1]) / tile_size;

        if (possiblePlacement(grid_x, grid_y))
        {
            doeZet(grid_x, grid_y);

            if (computer)  /* Laat de computer een random zet doen */
                AutoDoeZet();
        }
    }
}

void helpLaden(object o, EventArgs e){
    help = !help;  /* Switch help */
    scherm.Invalidate();
}

void doeZet(int x, int y){

    game_state[x, y] = current_player;
    checkIngeslotenStenen(x, y);
    current_player = current_player == 1 ? 2 : 1;  /* Switch player */

    if (!kanSpelerZetDoen()) {  /* Als de huidige speler geen zet kan doen ga naar terug naar de vorige speler */
        current_player = current_player == 1 ? 2 : 1;  /* Switch player */
        if (!kanSpelerZetDoen())
            eindeSpel();
    }

    scherm.Invalidate();
}

void AutoDoeZet(){

    int[,] mogelijke_pos = new int [tile_dim*tile_dim,2];
    int mogelijke_n = 0;

    for (int x = 0; x < tile_dim; x++)
    {
        for (int y = 0; y < tile_dim; y++)
        {
            if (possiblePlacement(x, y)) {
                mogelijke_pos[mogelijke_n, 0] = x;
                mogelijke_pos[mogelijke_n, 1] = y;
                mogelijke_n++;
            }
        }
    }

    /* Keis een random mogelijke zet */
    Random rnd = new Random();
    int rnd_int = rnd.Next(0, mogelijke_n);

    doeZet(mogelijke_pos[rnd_int,0], mogelijke_pos[rnd_int,1]);  /* Computer doet een zet */
}

bool possiblePlacement(int x, int y){

    /* Check if het mogelijk is om op x,y een tile te liggen voor 'current_player' */

    if (game_state[x,y] != 0)  /* Deze plek is al ingenomen door een speler */
        return false;

    for (int i = -1; i <= 1; i++)
    {
        for (int j = -1; j <= 1; j++)
        {
            if (x + i >= 0 && x + i < tile_dim && y + j >= 0 && y + j < tile_dim &&  /* Positie is niet out of bounds */
            otherPlayer(game_state[x+i,y+j]))  /* Positie is van de andere speler */
                return true;
        }
    }

    return false;
}


void checkIngeslotenStenen(int mid_x, int mid_y){

    for (int x_dir = -1; x_dir < 2; x_dir++)
    {
        for (int y_dir = -1; y_dir < 2; y_dir++)
        {
            if ((x_dir == 0 && y_dir == 0) ||  /* Direction verwijst naar zichzelf (dat kan niet) */
            (!checkBoundsGT(mid_x + x_dir, mid_y + y_dir) || !otherPlayer(game_state[mid_x + x_dir, mid_y + y_dir])))  /* Eerste positie moet van de andere speler zijn om een mogelijke ingesloten stenen te hebben */
                continue;

            for (int depth = 2; depth < tile_dim; depth++)
            {
                if (!checkBoundsGT(mid_x + x_dir*depth, mid_y + y_dir*depth) || game_state[mid_x + x_dir*depth, mid_y + y_dir*depth] == 0)  /* Is geen ingesloten stenen */
                    break;

                else if (game_state[mid_x + x_dir*depth, mid_y + y_dir*depth] == current_player)  /* Er zijn ingesloten stenen */
                {

                    for (int i = 1; i < depth; i++)
                        game_state[mid_x + x_dir*i, mid_y + y_dir*i] = current_player;  /* Update game_state */

                    break;
                }
            }
        }
    }
}

bool otherPlayer(int val){
    return val != 0 && val != current_player;  /* Geef terug of positie van de andere speler (de speler die niet aan de beurt is) */
}

bool checkBoundsGT(int x, int y){
    return x >= 0 && x < tile_dim && y >= 0 && y < tile_dim;  /* Geef terug of positie binnen 'game_state[]' valt */
}

bool kanSpelerZetDoen(){

    for (int x = 0; x < tile_dim; x++)
    {
        for (int y = 0; y < tile_dim; y++)
        {
            if (possiblePlacement(x, y))
                return true;
        }
    }

    return false;
}

void eindeSpel(){

    int[] stenen_aantal = stenenTeller();

    scherm.Invalidate();
    
    if (stenen_aantal[0] > stenen_aantal[1])
        MessageBox.Show($"{speler_namen[0]} heeft gewonnen!");
    else if (stenen_aantal[0] < stenen_aantal[1])
    {
        if (!computer)
            MessageBox.Show($"{speler_namen[1]} heeft gewonnen!");
        else
            MessageBox.Show($"computer ({speler_namen[1]}) heeft gewonnen!");
    }
    else
        MessageBox.Show("Remise!");
}
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
