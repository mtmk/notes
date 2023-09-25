#include <iostream>

using namespace std;

namespace tocpp_ch11 {

	struct Entity
	{
		string name;
		int number;

	};

	ostream& operator<<(ostream& os, const Entity& e)
	{
		return os << "{name:" << e.name << ", number:" << e.number << "}";
	}

	void run() {
		cout << "CH11 Input and Output" << endl << endl;

		Entity e{ "blah", 42 };

		cout << "e=" << e << endl;
	}
}