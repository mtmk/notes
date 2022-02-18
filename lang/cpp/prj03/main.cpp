#include <iostream>
#include <vector>

int main() {

	// Array
	{
		std::cout << std::endl;
		std::cout << "Array init" << std::endl;
		int arr[] = {1, 2, 3, 4};
		for (auto i: arr) {
			std::cout << i << std::endl;
		}
	
		std::cout << std::endl;
		std::cout << "Array init 2 (uninitialized)" << std::endl;
		int arr2[5];
		for (auto i: arr2) {
			std::cout << i << std::endl;
		}
	}

	// Vector
	{
		std::cout << std::endl;
		std::cout << "Vector init" << std::endl;
		std::vector<int> vec  {1, 2, 3, 4};
		for (auto i: vec) {
			std::cout << i << std::endl;
		}
	}

	// Pointers
	{
		std::cout << std::endl;
		std::cout << "Pointers" << std::endl;
		char cs[] = "Hi!";
		for (auto c: cs) {
			std::cout << c << std::endl;
		}
		char* p = &cs[1];
		std::cout << "pointer to cs[1] = " << *p << std::endl;
	}

}

