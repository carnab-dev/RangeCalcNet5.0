using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RangeCalc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private decimal ParseField(String textField)
        {
            if (String.IsNullOrEmpty(textField))
            {
                return 1;
            }
            else
            {
                string x = System.Text.RegularExpressions.Regex.Replace(textField, "[^0-9+.]", "9");
                return decimal.Parse(x);
            }
        }

        decimal d0;
        decimal d1;
        decimal p0;
        decimal m0;
        decimal m1;
        decimal r0;
        decimal r1;

        int ShotCount;

        private decimal RangeFormula(decimal Multiplier)
        {
            decimal n = (d0 * p0 * ShotCount - 100 / Multiplier) / (d0 * p0 * ShotCount - d1 * p0 * ShotCount) * (r1 - r0) + r0;

            
            if (n < r0) {
                return -1; // NaN
            }
            else if (n > r1)
            {
                return -2; // Inf.
            }
            else
            {
                return n;
            }
        }
        string FieldIteration(System.Windows.Forms.Label SubjectField, decimal Multiplier)
        {
            SubjectField.BackColor = SystemColors.ScrollBar;
            SubjectField.ForeColor = SystemColors.WindowText;

            switch (RangeFormula(Multiplier))
            {
                case -1:
                    SubjectField.Visible = true;
                    SubjectField.BackColor = SystemColors.ScrollBar;
                    return "";
                case -2:
                    SubjectField.Visible = true;
                    SubjectField.ForeColor = SystemColors.Highlight;
                    SubjectField.BackColor = SystemColors.Window;
                    return "∞";
                default:
                    SubjectField.Visible = true;
                    SubjectField.BackColor = SystemColors.Window;
                    SubjectField.ForeColor = SystemColors.WindowText;
                    return Convert.ToString(Decimal.Round(RangeFormula(Multiplier), 1));
            }
        }

        string FieldHide(System.Windows.Forms.Label SubjectField)
        {
            SubjectField.Visible = false;
            return "";
        }

        string FieldShow(System.Windows.Forms.Label SubjectField)
        {
            SubjectField.Visible = true;
            return "";
        }

        private void calculate_Click(object sender, EventArgs e)
        {
            d0 = ParseField(Damage0.Text);
            d1 = ParseField(Damage1.Text);
            p0 = ParseField(Pellet0.Text);
            m0 = ParseField(Multiplier0.Text);
            m1 = ParseField(Multiplier1.Text);
            r0 = ParseField(Range0.Text);
            r1 = ParseField(Range1.Text);

            System.Windows.Forms.Label[] OutputFieldsHead = new System.Windows.Forms.Label[] { Head0, Head1, Head2, Head3, Head4, Head5, Head6, Head7, Head8, Head9 };
            System.Windows.Forms.Label[] OutputFieldsTorso = new System.Windows.Forms.Label[] { Torso0, Torso1, Torso2, Torso3, Torso4, Torso5, Torso6, Torso7, Torso8, Torso9 };
            System.Windows.Forms.Label[] OutputFieldsLimb = new System.Windows.Forms.Label[] { Limb0, Limb1, Limb2, Limb3, Limb4, Limb5, Limb6, Limb7, Limb8, Limb9 };
            System.Windows.Forms.Label[] OutputFieldsShot = new System.Windows.Forms.Label[] { Shot0, Shot1, Shot2, Shot3, Shot4, Shot5, Shot6, Shot7, Shot8, Shot9 };

            ErrorLabel.Visible = false;

            try
            {
                int OutputCutoff = 0;

                for (ShotCount = 1; ShotCount < 11; ShotCount++)
                {
                    if (OutputCutoff == 1)
                    {
                        OutputFieldsHead[ShotCount - 1].Text = FieldHide(OutputFieldsHead[ShotCount - 1]);
                        OutputFieldsTorso[ShotCount - 1].Text = FieldHide(OutputFieldsTorso[ShotCount - 1]);
                        OutputFieldsLimb[ShotCount - 1].Text = FieldHide(OutputFieldsLimb[ShotCount - 1]);
                        FieldHide(OutputFieldsShot[ShotCount - 1]);
                    } else
                    {
                        OutputFieldsHead[ShotCount - 1].Text = FieldIteration(OutputFieldsHead[ShotCount - 1], m0);
                        OutputFieldsTorso[ShotCount - 1].Text = FieldIteration(OutputFieldsTorso[ShotCount - 1], m1);
                        OutputFieldsLimb[ShotCount - 1].Text = FieldIteration(OutputFieldsLimb[ShotCount - 1], 1);
                        FieldShow(OutputFieldsShot[ShotCount - 1]);
                    }

                    if (OutputFieldsHead[ShotCount - 1].Text == "∞" && OutputFieldsTorso[ShotCount - 1].Text == "∞" && OutputFieldsLimb[ShotCount - 1].Text == "∞") {
                        OutputCutoff = 1;
                    };
                } 
            }
            catch(DivideByZeroException error)
            {
                ErrorLabel.Visible = true;
            }
        }
    }
}
