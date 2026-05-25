
namespace AppMaui.Core.DTOs
{
    /// <summary>
    /// Disponibiliza os símbolos e caracteres utilizados
    /// na geração dos criptogramas da aplicação.
    /// </summary>
    public static class CriptogramaSimbolos
    {
        /// <summary>
        /// Lista de símbolos utilizados na substituição
        /// das letras dos criptogramas.
        /// Os códigos representam ícones FontAwesome.
        /// </summary>
        public static readonly string[] Lista =
            {
                    "\uf005", // star
                    "\uf004", // heart
                    "\uf0f3", // bell
                    "\uf06d", // fire
                    "\uf0e7", // bolt
                    "\uf043", // tint
                    "\uf001", // music
                    "\uf0c2", // cloud
                    "\uf185", // sun
                    "\uf186", // moon
                "\uf554", // clock
                "\uf11b", // gamepad
                "\uf135", // rocket
                "\uf188", // bug
                "\uf1b0", // tree
                "\uf3a5", // gem
                "\uf521", // crown
                "\uf72b", // dice
                "\uf7d9", // ghost
                "\uf4d8", // book
                    "\uf49e", // atom
                    "\uf863", // yin-yang
                    "\uf578", // fish
                    "\uf6be", // cat
                    "\uf6d3", // dog
                    "\uf84a", // dragon
                    "\uf7fb", // brain
                    "\uf0ac", // globe
                    "\uf72e", // chess
                    "\uf555", // stopwatch
                "\uf70c", // feather
                "\uf5dc", // mountain
                "\uf6ec", // hamburger
                "\uf7a2", // mask
                "\uf5fd", // meteor
                "\uf1fd", // paint brush
                "\uf53f", // palette
                "\uf8ff", // pizza
                "\uf810", // robot
                "\uf433"  // shield
            };
        /// <summary>
        /// Caracteres suportados na geração dos criptogramas.
        /// </summary>
        public static readonly string[] Alfabeto =
            {
                   "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
                   "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
                   "U", "V", "W", "X", "Y", "Z", "Ç", "Á", "É", "Í", 
                   "Ó", "Ú", "Â", "Ê", "Ô", "Ã", "Õ"
            };
        }
}
