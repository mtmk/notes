#include <iostream>
#include <string>

using namespace std;
using namespace std::literals::string_view_literals;

namespace tocpp_ch10
{
	string cat(string_view sv1, string_view sv2)
	{
		string res{ sv1 };
		return res += sv2;
	}

	void run()
	{
		cout << "CH10 Strings and Regex" << endl << endl;

		string king = "Harold";

		auto s1 = cat(king, "Will");
		cout << s1 << endl;

		auto s2 = cat(king, king);
		cout << s2 << endl;

		auto s3 = cat("Joe", "Blogs"sv);
		cout << s3 << endl;
	}
}