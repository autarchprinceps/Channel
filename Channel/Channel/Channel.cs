﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Channel
{
    public class Channel<T>
    {
        private Queue<T> buffer;
        private bool limited;
        private int max;

        public T take()
        {
            T result;
            while (true)
            {
                Monitor.Enter(buffer);
                if (buffer.Count > 0)
                {
                    result = buffer.Dequeue();
                    break;
                }
                else
                {
                    Monitor.Exit(buffer);
                    Monitor.PulseAll(buffer);
                    Thread.Sleep(50);
                }
            }
            Monitor.Exit(buffer);
            Monitor.PulseAll(buffer);
            return result;
        }

        public void give(T value)
        {
            if (limited)
            {
                while (true)
                {
                    Monitor.Enter(buffer);
                    if (buffer.Count <= max)
                    {
                        buffer.Enqueue(value);
                        break;
                    }
                    else
                    {
                        Monitor.Exit(buffer);
                        Monitor.PulseAll(buffer);
                        Thread.Sleep(50);
                    }
                }
            }
            else
            {
                Monitor.Enter(buffer);
                buffer.Enqueue(value);
            }
            Monitor.Exit(buffer);
            Monitor.PulseAll(buffer);
        }

        public Channel()
        {
            buffer = new Queue<T>();
        }

        public Channel(int MaxSize)
        {
            limited = true;
            buffer = new Queue<T>(MaxSize);
            max = MaxSize;
        }
    }
}
