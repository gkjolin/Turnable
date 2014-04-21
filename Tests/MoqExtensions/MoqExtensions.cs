using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests.MoqExtensions
{
    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}

