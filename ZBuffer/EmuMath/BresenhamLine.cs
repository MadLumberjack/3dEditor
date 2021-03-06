﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmuEngine.Shapes;

namespace EmuEngine.EmuMath
{
    class BresenhamLine : ILineRasterizer
    {
        public List<MPoint> GetLine(MPoint point1, MPoint point2)
        {
            return Bresenham3D((int)point1.X, (int)point1.Y, (int)point1.Z, (int)point2.X, (int)point2.Y, (int)point2.Z);
        }

        private List<MPoint> Bresenham3D(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            var points = new List<MPoint>();

            float x0 = x1, y0 = y1, z0 = z1;
            int i, dx, dy, dz, l, m, n, x_inc, y_inc, z_inc, err_1, err_2, dx2, dy2, dz2;
            int[] point = new int[3];

            point[0] = x1;
            point[1] = y1;
            point[2] = z1;

            dx = x2 - x1;
            dy = y2 - y1;
            dz = z2 - z1;

            x_inc = (dx < 0) ? -1 : 1;
            l = Math.Abs(dx);

            y_inc = (dy < 0) ? -1 : 1;
            m = Math.Abs(dy);

            z_inc = (dz < 0) ? -1 : 1;
            n = Math.Abs(dz);

            dx2 = l << 1;
            dy2 = m << 1;
            dz2 = n << 1;

            if ((l >= m) && (l >= n))
            {
                err_1 = dy2 - l;
                err_2 = dz2 - l;
                for (i = 0; i < l; i++)
                {
                    points.Add(new MPoint(point[0], point[1], point[2]));

                    if (err_1 > 0)
                    {
                        point[1] += y_inc;
                        err_1 -= dx2;
                    }
                    if (err_2 > 0)
                    {
                        point[2] += z_inc;
                        err_2 -= dx2;
                    }
                    err_1 += dy2;
                    err_2 += dz2;
                    point[0] += x_inc;
                }
            }
            else if ((m >= l) && (m >= n))
            {
                err_1 = dx2 - m;
                err_2 = dz2 - m;
                for (i = 0; i < m; i++)
                {
                    points.Add(new MPoint(point[0], point[1], point[2]));

                    if (err_1 > 0)
                    {
                        point[0] += x_inc;
                        err_1 -= dy2;
                    }
                    if (err_2 > 0)
                    {
                        point[2] += z_inc;
                        err_2 -= dy2;
                    }
                    err_1 += dx2;
                    err_2 += dz2;
                    point[1] += y_inc;
                }
            }
            else
            {
                err_1 = dy2 - n;
                err_2 = dx2 - n;
                for (i = 0; i < n; i++)
                {
                    points.Add(new MPoint(point[0], point[1], point[2]));

                    if (err_1 > 0)
                    {
                        point[1] += y_inc;
                        err_1 -= dz2;
                    }
                    if (err_2 > 0)
                    {
                        point[0] += x_inc;
                        err_2 -= dz2;
                    }
                    err_1 += dy2;
                    err_2 += dx2;
                    point[2] += z_inc;
                }
            }

            foreach (MPoint pointe in points)
            {
                pointe.ARGB = 16711680;
            }
            return points;
        }

        private void Swap<T>(ref T l, ref T r)
        {
            T temp = l;

            l = r;

            r = temp;
        }
    }
}
