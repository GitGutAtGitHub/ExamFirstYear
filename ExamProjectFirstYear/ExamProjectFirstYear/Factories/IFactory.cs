using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	interface IFactory
	{
		GameObject Create(Tag type);
	}
}
