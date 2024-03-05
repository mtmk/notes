#include <stdio.h>
#include "sha256.h"

void main()
{
	SHA256_CTX ctx;
	BYTE data[] = "Hello World!";
	BYTE hash[32];

	sha256_init(&ctx);
	sha256_update(&ctx, data, strlen(data));
	sha256_update(&ctx, data, strlen(data));
	sha256_update(&ctx, "ABC", strlen("ABC"));
	sha256_final(&ctx, hash);

	for (int i = 0; i < 32; i++)
	{
		printf("%02x", hash[i]);
	}

	printf("\n");
}
