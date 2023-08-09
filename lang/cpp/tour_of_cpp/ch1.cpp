#include <iostream>
#include <vector>
#include <math.h>

//import std;

consteval int64_t do_it(int64_t i) { return i + i; }

constexpr int64_t do_it2(int64_t i) { return i + i; }

double square(double x)
{
	return x * x;
}

void print_square(double x)
{
	using namespace std;
	cout << "square of " << x << " is " << square(x) << "\n";
}

constexpr int ce = 5 * 1024 * 1024;

int count_x(const char* p, char x)
{
	//if (p == nullptr)
	if (p)
		return 0;

	auto count = 0;

	//for (; *p != 0; p++)
	while (*p)
	{
		if (*p == x)
			count++;
		p++;
	}

	return count;
}

bool accept()
{
	std::cout << "proceed (y/n)? ";
	char answer;
	std::cin >> answer;

	switch (answer)
	{
	case 'y':
		return true;
	case 'n':
		return false;
	default:
		std::cout << "no?" << std::endl;
		return false;
	}

	//if (answer == 'y')
	//	return true;
	//return false;
}

void ch1()
{
	const double x1 = sqrt(36);

	std::vector<int> v { 1, 2, 3 };

	double d { 1.1 };

	auto dd = square(d);

	using namespace std;
	
	cout << "cont x1 " << x1 << endl;
	
	cout << "contexpr " << ce << endl;

	cout << "hi\n" << do_it(0b00001111) << " " << do_it(0xbadbeef) << " " << dd << endl;
	
	print_square(d);

	char cv[6];
	cv[1] = 'A';
	char* cp;
	cp = &cv[1];
	auto c1 = *cp;

	cout << "c1 " << c1 << endl;

	int iv2[] { 10, 11, 12, 13 };

	cout << "iv2 ";
	for (auto i = 0; i != 4; i++)
	{
		cout << iv2[i] << ", ";
	}
	cout << endl;

	for (auto& i : iv2) { i++; }

	cout << "iv2 ";
	for (auto i : iv2)
	{
		cout << i << ", ";
	}
	cout << endl;

	cout << "count x " << count_x("asdasd", 'a') << endl;

	auto a = accept();

	cout << "accept " << a << endl;

	int x = 7;
	int& r{ x };
	r = 5;
	cout << "r " << r << endl;
	
	// int& r2; // error not initialized



}