using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingSite.Controllers;

namespace BowlerTest
{
    [TestClass]
    public class UnitTest1
    {   
        //Sorry for bad imagination for tests! 

        [TestMethod]
        public void PerfectGameTest()
        {
            HomeController controller = new HomeController();
            List<Frame> fList = new List<Frame>();
            Frame[] result = new Frame[0];

            for (int i = 1; i <= 10; i++)
            {
                Frame frame = new Frame
                {
                    FrameNumber = i,
                    Rolls = new Rolls
                    {
                        First = 10,
                        Second = 0,
                        Third = 0
                    },
                    Score = 0
                };

                if (i == 10)
                {
                    frame.Rolls.Second = 10;
                    frame.Rolls.Third = 10;
                }

                fList.Add(frame);
                result = controller.RegisterScore(fList.ToArray());
            }

            Assert.IsTrue(result[result.Length - 1].Score == 300); 
        }
        [TestMethod]
        public void StandardGameTest()
        {
            HomeController controller = new HomeController();
            List<Frame> frameList = new List<Frame>();
            Frame[] frames = new Frame[0];
            Random rand = new Random();


            for (int i = 1; i <= 10; i++)
            {
                int first = rand.Next(10); //To produce valid frames

                Frame frame = new Frame
                { 
                    FrameNumber = i,
                    Rolls = new Rolls
                    {
                        First = first,
                        Second = rand.Next(10) - first,
                        Third = 0
                    },
                    Score = 0
                };

                if (i == 10)
                {
                    frame.Rolls.Second = rand.Next(10);
                    frame.Rolls.Third = rand.Next(10);
                }

                frameList.Add(frame);
                frames = controller.RegisterScore(frameList.ToArray());
            }

            Assert.IsTrue(frames[frames.Length - 1].Score <= 300);
        }
    }
}
