#include <iostream>
#include <chrono>
#include <math.h>

using int128 = __int128_t;

auto f(int128 n) {
	int128 total  = 0;
    int128 c      = 1; 
    int128 c3     = 1; // current cubic value of c
    int128 c3bias = 1; // bias between the difference of successive cubic numbers 
	while (c3 <= n - 2) {
		const int128 b = std::floor(std::sqrt(static_cast<double>(n - 1 - c3)));
        total -= b * (1 + 6.0 * (c3 - n) + b * (2.0 * b + 3.0)) / 6.0;

        c3 += c3bias += 6 * c; // increse the bias and set the next value of c ^ 3
        ++c;
    }
	return total;
}

int main() {
    const auto t1 = std::chrono::high_resolution_clock::now();
    std::cout << "Result: " << static_cast<double>(f(std::pow(10, 18))) << '\n';
    const auto t2 = std::chrono::high_resolution_clock::now();

    const float elapsed = std::chrono::duration_cast<std::chrono::milliseconds>(t2 - t1).count();

    std::cout << "Elapsed: " << elapsed << "ms\n";
    return 0;
}