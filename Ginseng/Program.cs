using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 산삼
{
    public class Point
    {
        int x;
        int y;
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point() { }


    }
    public class Program
    {
        static List<Point> startPoint = new List<Point>();
        const int MaxSize = 12;
        const int minLoc = 4;   // minLoc는 산삼의 좌측 한계선
        const int maxLoc = 7;   // maxLoc는 산삼의 우측 한계선

        static void Main(string[] args)
        {
            char[,] ginseng = new char[MaxSize, MaxSize];
            Random random = new Random();
            
            List<Point> direction = new List<Point>();
            direction.Add(new Point(-1, 0));    //좌
            direction.Add(new Point(1, 0));     //우
            direction.Add(new Point(0, 1));     //하
            direction.Add(new Point(0, -1));    //상
            /*
            Point[] direction = new Point[4] {
                new Point(-1,0),     //좌
                new Point(1, 0),     //우
                new Point(0, 1),     //남
                new Point(0, -1)     //북
            };
            */

            //static List<Point> startPoint = new List<Point>();
            StartPointPush(ref startPoint);

            // 뿌리의 개수는 0~6사이의 난수를 발생시킨다.
            int rootNum = random.Next(0, 7);

            Init(ref ginseng);

            Console.WriteLine("뿌리의 개수 : " + rootNum);

            for (int i = 0; i < rootNum; i++)
            {
                DeleteRoot(ref ginseng, ref startPoint);
                BFS(ref ginseng, ref startPoint, direction);

                int startPointRand = random.Next(startPoint.Count);
                Point randomRoot = new Point();
                randomRoot.X = startPoint[startPointRand].X;
                randomRoot.Y = startPoint[startPointRand].Y;
                int rootLength = random.Next(0, 4) + 6;
                /*
                Console.WriteLine("{0}주기", (i + 1));
                foreach(Point data in startPoint)
                {
                    Console.WriteLine("x : {0}, y : {1}", data.X, data.Y);
                }
                Console.WriteLine("시작위치개수 : {0}", startPoint.Count);
                Console.WriteLine("시작 위치 : {0}, {1}", randomRoot.X, randomRoot.Y);
                */
                CreateRoot(ref ginseng, direction,ref randomRoot, rootLength, 1, 0);
            }

            Print(ref ginseng);
        }

        // 뿌리를 생성하는 함수 CreateRoot
        static void CreateRoot(ref char[,] map , List<Point> direction,ref Point randomRoot, int rootLength,int order,int frontRand)
        {
            //뿌리 길이가 0 미만일경우 return
            if (rootLength <= 0)
                return;
            Random random = new Random();
            List<Point> copyDirection = new List<Point>();
            copyDirection = direction.ToList<Point>();
            int rand = random.Next(4);
            Point dir = new Point(); ;
            int i = 0;

            for (i = 0; i < 4; i++)
            {
                if (order == 1)
                {
                    if (randomRoot.X == 3)
                    {
                        dir = copyDirection[0];
                    }
                    else if (randomRoot.X == 8)
                        dir = copyDirection[1];
                    else if (randomRoot.Y == 3)
                        dir = copyDirection[3];
                    else if (randomRoot.Y == 8)
                        dir = copyDirection[2];
                    else
                        dir = copyDirection[rand];
                }
                else
                {
                    dir = copyDirection[rand];  // 랜덤 위치를 랜덤으로 선택
                }
                // 안 막혀있을 시
                if (((randomRoot.X + dir.X) > 0 && (randomRoot.X + dir.X < 12)) && ((randomRoot.Y + dir.Y > 0) && (randomRoot.Y + dir.Y < 12)))
                {
                    if (map[randomRoot.X + dir.X, randomRoot.Y + dir.Y] == '0')
                    {
                        // 연산자 오버로딩으로 대체 가능
                        map[randomRoot.X, randomRoot.Y] = (char)(48 + order);
                        randomRoot.X += dir.X;
                        randomRoot.Y += dir.Y;
                        break;
                    }
                }
                // 막혀있을 시
                copyDirection.RemoveAt(rand);
                rand = random.Next(3 - i);
            }
            if (i == 4) //4방향 전부 막힌 경우
                return;
            CreateRoot(ref map, direction, ref randomRoot, --rootLength, ++order, rand);
        }

        // 시작 포인트를 리스트에 담아주는 함수 StartPointPush
        static void StartPointPush(ref List<Point> startPoint)
        {
            for (int j = 0; j < 4; j++)
            {
                startPoint.Add(new Point(minLoc - 1, minLoc + j));
                startPoint.Add(new Point(minLoc + j, minLoc - 1));
                startPoint.Add(new Point(minLoc + j, maxLoc + 1));
                startPoint.Add(new Point(maxLoc + 1, minLoc + j));
            }
        }

        // 뻗어나갈 부분이 있는지 체크하며, 없으면 삭제하는 함수 DeleteRoot
        static void DeleteRoot(ref char[,] map, ref List<Point> startPoint)
        {
            for (int pt = 0; pt < startPoint.Count; pt++)
            {
                //Console.WriteLine("X : {0} Y : {1}", startPoint[pt].X, startPoint[pt].Y);
                if (map[startPoint[pt].X, startPoint[pt].Y] != '0')
                {
                    //startPoint.Remove(startPoint[pt]);
                    startPoint.RemoveAt(pt);
                    continue;
                }
                // 좌
                if (startPoint[pt].X == 3)
                {
                    //Console.WriteLine("A");
                    if (map[startPoint[pt].X - 1, startPoint[pt].Y] != '0')
                    {
                        //Console.WriteLine("a");
                        //startPoint.Remove(startPoint[pt]);
                        startPoint.RemoveAt(pt);
                        continue;
                    }
                }
                // 우
                if (startPoint[pt].Y == 3)
                {
                    //Console.WriteLine("B");
                    if (map[startPoint[pt].X, startPoint[pt].Y - 1] != '0')
                    {
                        //Console.WriteLine("b");
                        //startPoint.Remove(startPoint[pt]);
                        startPoint.RemoveAt(pt);
                        continue;
                    }
                }
                // 남
                if (startPoint[pt].X == 8)
                {
                    //Console.WriteLine("C");
                    if (map[startPoint[pt].X + 1, startPoint[pt].Y] != '0')
                    {
                        //Console.WriteLine("c");
                        //startPoint.Remove(startPoint[pt]);
                        startPoint.RemoveAt(pt);
                        continue;
                    }
                }
                // 북
                if (startPoint[pt].Y == 8)
                {
                    //Console.WriteLine("D");
                    if (map[startPoint[pt].X, startPoint[pt].Y + 1] != '0')
                    {
                        //Console.WriteLine("d");
                        //startPoint.Remove(startPoint[pt]);
                        startPoint.RemoveAt(pt);
                        continue;
                    }
                }
            }
        }

        static bool BFS(ref char[,] map, ref List<Point> startPoint, List<Point> direction)
        {
            
            int n = map.Length - 13;

            // 반복하여 확인
            for (int i = 0; i < startPoint.Count; i++)
            {
                Queue<Point> queue = new Queue<Point>();
                queue.Enqueue(new Point(startPoint[i].X, startPoint[i].Y));

                // 방문한 곳 체크
                int[,] visit = new int[12, 12];
                visit[startPoint[i].X, startPoint[i].Y]++;  //이미 지나갔다는 표시

                Point temp = new Point(startPoint[i].X, startPoint[i].Y);
                while (queue.Count != 0 && queue.Count <= 6)
                {
                    // 4방향 체크
                    // 좌
                    temp = new Point(temp.X, temp.Y);
                    if (temp.X > 0)
                    {
                        if ((map[temp.X + direction[0].X, temp.Y + direction[0].Y] == '0' ) && (visit[temp.X+direction[0].X,temp.Y+direction[0].Y] == 0))
                        {
                            temp.X += direction[0].X;
                            temp.Y += direction[0].Y;
                            queue.Enqueue(temp);
                            visit[temp.X, temp.Y]++;
                            continue;
                        }
                    }
                    // 우
                    if (temp.X < 11)
                    {
                        if ((map[temp.X + direction[1].X, temp.Y + direction[1].Y] == '0') && (visit[temp.X + direction[1].X, temp.Y + direction[1].Y] == 0))
                        {
                            temp.X += direction[1].X;
                            temp.Y += direction[1].Y;
                            queue.Enqueue(temp);
                            continue;
                        }
                    }
                    // 남
                    if (temp.Y < 11)
                    {
                        if ((map[temp.X + direction[2].X, temp.Y + direction[2].Y] == '0') && (visit[temp.X + direction[2].X, temp.Y + direction[2].Y] == 0))
                        {
                            temp.X += direction[2].X;
                            temp.Y += direction[2].Y;
                            queue.Enqueue(temp);
                            continue;
                        }
                    }
                    // 북
                    if (temp.Y > 0)
                    {
                        if ((map[temp.X + direction[3].X, temp.Y + direction[3].Y] == '0')  && (visit[temp.X + direction[3].X, temp.Y + direction[3].Y] == 0))
                        {
                            
                            temp.X += direction[3].X;
                            temp.Y += direction[3].Y;
                            queue.Enqueue(temp);
                            continue;
                        }
                    }
                }
                if (queue.Count < 6)
                    startPoint.RemoveAt(i);
            }

            //시작 지점이 1곳 이상 남았을 경우
            if (startPoint.Count != 0)
                return true;
            else
                return false;
        }

        // 산삼의 시작 좌표를 잡아줄 함수 RootStart
        static Point RootStart(ref char[,] map, Random r)
        {
            Point p = new Point(r.Next(minLoc) + minLoc, r.Next(minLoc) + minLoc);

            return p;
        }

        // 산삼의 기초를 만들어주는 함수 Init
        static void Init(ref char[,] arr)
        {
            for (int i = 0; i < arr.Length / MaxSize; i++)
            {
                for (int j = 0; j < arr.Length / MaxSize; j++)
                {
                    if ((i >= minLoc && i <= maxLoc) && (j >= minLoc && j <= maxLoc))
                    {
                        arr[i, j] = '산';
                    }
                    else
                    {
                        arr[i, j] = '0';
                    }
                }
            }
        }
        // 산삼을 출력해주는 함수 Print
        static void Print(ref char[,] arr)
        {
            for (int i = 0; i < MaxSize; i++)
            {
                for (int j = 0; j < MaxSize; j++)
                {
                    Console.Write(arr[i, j] + "\t");
                }
                Console.WriteLine("\n");
            }
        }
    }
}
