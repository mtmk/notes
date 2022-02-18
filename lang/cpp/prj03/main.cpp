#include <iostream>
#include <vector>

void doubleIt(std::vector<int>& v) {
	for (auto& i : v) i *= 2;
}

int main() {

	// Array
	{
		std::cout << "Array init: ";
		int arr[] = {1, 2, 3, 4};
		for (auto i : arr) {
			std::cout << i << " ";
		}
		std::cout << std::endl;
	
		std::cout << "Array init (uninitialized): ";
		int arr2[5];
		for (auto i : arr2) {
			std::cout << i << " ";
		}
		std::cout << std::endl;
	}

	// Vector
	{
		std::cout << "Vector init: ";
		std::vector<int> vec  {1, 2, 3, 4};
		for (auto i : vec) {
			std::cout << i << " ";
		}
		std::cout << std::endl;
	}

	// Pointers
	{
		std::cout << "Pointers" << std::endl;
		char cs[] = "Hi!";
		std::cout << " arr: ";
		for (auto c : cs) {
			std::cout << c << " ";
		}
		std::cout << std::endl;
		char* p = &cs[1];
		std::cout << " pointer to cs[1] = " << *p << std::endl;
	}

	// References
	{
		std::cout << "References" << std::endl;
		int arr[] = {1, 2, 3, 4, 5};
		std::cout << " arr: ";
		for (auto& i : arr) {
			std::cout << i << " ";
			i = i * i;
		}
		std::cout << std::endl;
		std::cout << " sqr: ";
		for (auto i : arr) {
			std::cout << i << " ";
		}
		std::cout << std::endl;

		std::cout << "Reference func args" << std::endl;
		std::vector<int> v {1, 2, 3, 4};
		std::cout << "     v = ";
		for (auto i : v) std::cout << i << " ";
		std::cout << std::endl;

		doubleIt(v);
		std::cout << " 2 x v = ";
		for (auto i : v) std::cout << i << " ";
		std::cout << std::endl;
	}

}

