using System;
using System.Collections.Generic;
using System.Text;

namespace Utility{
    class ConsoleMenue{
        private const string SELECTED_INDICATOR = " <--";
        private const string MSG_OPTION_SEPARATOR = "---------------------";
        private string[] options;
        private string msg;

        public ConsoleMenue(string msg, string[] options)
        {
            this.options = options;
            this.msg = msg;
        }

        public int getUserSelection(){
            // by default select the first menue item
            int selection = 0;
            int len = options.Length;

            // look refreshes the interface
            while (true) {
                Console.Clear();
                printOptions(selection);
                System.ConsoleKey key = catchKeyPress(); //dummy data

                if (key == ConsoleKey.Enter) {
                    break;
                }
                else if (key == ConsoleKey.UpArrow) {
                    selection--;
                    if (selection < 0)
                        selection = len - 1;
                }
                else if (key == ConsoleKey.DownArrow) {
                    selection = (selection + 1) % len;
                }
            }

            return selection;
        }

        private System.ConsoleKey catchKeyPress() {
            // wait for the user to press a key
            while (!Console.KeyAvailable) {
                // do nothing
            }

            // get the pressed key and take necessary steps
            return Console.ReadKey().Key;
        }

        private void printOptions(int selected)
        {
            Console.WriteLine(this.msg);
            Console.WriteLine(MSG_OPTION_SEPARATOR);
            for (int i = 0, len = options.Length; i < len; i++) {
                Console.Write(options[i]);
                if (selected == i)
                    Console.Write(SELECTED_INDICATOR);
                Console.Write("\n");
            }
        }
    }
}
