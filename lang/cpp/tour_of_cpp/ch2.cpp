#include <iostream>

using namespace std;

namespace tocpp_ch2
{
	struct Vector
	{
	public:
		Vector(int s) : elem{ new double[s] }, sz{ s } {}

		double& operator[](int i)
		{
			return elem[i];
		}

		int size() { return sz; }

	private:
		double* elem;
		int sz;
	};

	double read_and_sum(int s)
	{
		Vector v{ 3 };

		for (int i = 0; i != s; i++)
		{
			cout << "enter a number: ";
			cin >> v[i];
		}

		double sum = 0;
		for (auto i = 0; i != s; i++)
		{
			sum += v[i];
		}

		return sum;
	}

	enum class Color { red, blue, green };

	enum class Traffic_light
	{
		green, yellow, red
	};

	Traffic_light& operator++(Traffic_light& t)                // prefix increment: ++
	{
		switch (t) {
		case Traffic_light::green:         return t = Traffic_light::yellow;
		case Traffic_light::yellow:        return t = Traffic_light::red;
		case Traffic_light::red:             return t = Traffic_light::green;
		}
	}

	ostream& operator<<(ostream& os, Traffic_light& t)                // prefix increment: ++
	{
		using enum Traffic_light;
		switch (t) {
		case green:
			os << "green";
			break;
		case yellow:
			os << "yellow";
			break;
		case red:
			os << "red";
			break;
		default:
			os << int(t);
		}
		return os;
	}

	enum Color2 { red, green, blue };

	void run()
	{
		cout << "A Tour of C++, Chapter 2" << endl;

		auto sum = read_and_sum(3);
		cout << "Result: " << sum << endl;

		Color col = Color::red;
		Traffic_light light = Traffic_light::red;

		cout << "traffic ligth 1: " << light << endl;
		++light;
		cout << "traffic ligth 2: " << light << endl;
		cout << "traffic ligth 3: " << ++light << endl;
		cout << "traffic ligth 4: " << ++light << endl;
		cout << "traffic ligth 5: " << ++light << endl;

		Color x = Color{ 6 };
		Color y{ 6 };

		int x1 = int(Color::red);

		Traffic_light t2{ 6 };
		cout << "traffic ligth t2 = " << t2 << endl;

		Color2 c2{ red };
		cout << "color2:red = " << c2 << endl;
	}
}
