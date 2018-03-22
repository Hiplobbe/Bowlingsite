using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BowlingSite.Controllers
{
    public class HomeController : Controller
    {
        private Frame[] Frames = new Frame[10];
        private static int Round = 1;        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpPost]
        public Frame[] RegisterScore ([FromBody]Frame[] frames)
        {
            foreach (Frame frame in frames)
            {
                if (Frames[frame.FrameNumber-1] == null)
                {
                    frame.Mark = SetMark(frame);
                    Frames[frame.FrameNumber-1] = frame;
                }
            }

            if (ValidateRolls() && ValidateMarks())
            {
                SetScore();

                if (Round > 10)
                {
                    Round = 0;
                }

                return Frames.Where(x => x != null).ToArray();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Validates the marks of the frames.
        /// </summary>
        /// <returns>Returns false if not valid</returns>
        private bool ValidateMarks()
        {
            foreach (Frame frame in Frames)
            {
                if (frame == null)
                {
                    break;
                }
                else if(frame.Rolls.First == 10 && frame.Mark != Mark.Strike)
                {
                    return false;
                }
                else if(frame.Rolls.First != 10 && frame.Rolls.First + frame.Rolls.Second == 10 && frame.Mark != Mark.Spare)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Validates the scores for the rolls.
        /// </summary>
        /// <returns> Returns false if not valid</returns>
        private bool ValidateRolls()
        {
            foreach (Frame frame in Frames)
            {
                if(frame == null)
                {
                    break;
                }
                else if ((frame.Rolls.First + frame.Rolls.Second) > 10 && frame.FrameNumber != 10)
                {
                    return false;
                }
                else if(frame.Rolls.First > 10 || frame.Rolls.Second > 10 || frame.Rolls.Third > 10)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Sets the score for all frames.
        /// </summary>
        private void SetScore()
        {
            int score = 0;

            for (int i = 0; i <= Round - 1; i++)
            {
                if (i == 9)
                {
                    score += CalcBonusRound(Frames[i]);
                }
                else if (i == Round - 1)
                {
                    score += Frames[i].Rolls.GetSummary();
                }
                else if (Frames[i + 1] != null && Frames[i].Mark == Mark.Strike)
                {
                    score += 10 + Frames[i+1].Rolls.GetSummary();

                    if(i + 2 <= Round - 1 && i != 7)
                    {
                        score += Frames[i+2].Rolls.GetSummary();
                    }
                    //Frame 8 special case since it only count the first throw in Frame 10
                    else if (i == 7 && Round == 10)
                    {
                        score += Frames[i + 2].Rolls.First;
                    }
                }
                else if (Frames[i + 1] != null && Frames[i].Mark == Mark.Spare)
                {
                    score += 10 + Frames[i + 1].Rolls.First;
                }
                else if (Frames[i] != null && Frames[i].Mark == Mark.Open)
                {
                    score += Frames[i].Rolls.GetSummary();
                }
                else
                {
                    score += 0;
                }

                Frames[i].Score = score;
            }

            Round++;
        }
        /// <summary>
        /// Sets the correct mark value for the frame.
        /// </summary>
        /// <param name="frame">The frame which mark to set.</param>
        private Mark SetMark(Frame frame)
        {
            if (frame.Rolls.First == 10)
            {
                return Mark.Strike;
            }
            else if (frame.Rolls.First + frame.Rolls.Second == 10)
            {
                return Mark.Spare;
            }
            else
            {
                return Mark.Open;
            }
        }
        /// <summary>
        /// Calculates the score for the 10th frame.
        /// </summary>
        /// <param name="frame">The 10th frame</param>
        /// <returns>The score value</returns>
        internal int CalcBonusRound(Frame frame)
        {
            if(frame.Rolls.First == 10)
            {
               return 10 + (frame.Rolls.Second + frame.Rolls.Third);
            }
            else if(frame.Rolls.First + frame.Rolls.Second == 10)
            {
                return 10 + (frame.Rolls.Third);
            }
            else
            {
                return frame.Rolls.First + frame.Rolls.Second;
            }
        }
    }

    public class Frame
    {
        public int FrameNumber { get; set; }
        public Mark Mark { get; set; }
        public Rolls Rolls { get; set; }        
        public int Score { get; set; }
    }
    public class Rolls
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }

        /// <summary>
        /// Returns the accumulated score of all the rolls of the frame.
        /// </summary>
        public int GetSummary()
        {
            return First + Second;
        }
    }
    public enum Mark
    {
        Strike,
        Spare,
        Open
    }
}
