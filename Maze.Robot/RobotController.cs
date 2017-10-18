using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        private bool reachedEnd = false;
        private List<Point> points;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
            points = new List<Point>();
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            robot.ReachedExit += (_, __) => reachedEnd = true;

            check(new Point(0, 0));

            if (!reachedEnd)
            {
                robot.HaltAndCatchFire();
            }
        }

        public void check(Point point)
        {
            if (!points.Contains(point) && !reachedEnd)
            {
                points.Add(point);
                if (robot.TryMove(Direction.Left))
                {
                    check(new Point(point.X - 1, point.Y));
                    if (!reachedEnd) {
                        robot.Move(Direction.Right);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Right))
                {
                    check(new Point(point.X + 1, point.Y));
                    if (!reachedEnd) {
                        robot.Move(Direction.Left);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Down))
                {
                    check(new Point(point.X, point.Y + 1));
                    if (!reachedEnd) {
                        robot.Move(Direction.Up);
                    }
                }

                if (!reachedEnd && robot.TryMove(Direction.Up))
                {
                    check(new Point(point.X, point.Y - 1));
                    if (!reachedEnd) {
                        robot.Move(Direction.Down);
                    }
                }
            }
        }
    }
}