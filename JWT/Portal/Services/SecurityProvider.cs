using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Portal.Services
{
    public class SecurityProvider
    {
        private readonly RSA rsa;

        //Store the constant rsa key in xml
        private const string rsaKeyXml = "<RSAKeyValue>" +
            "<Modulus>uviGqaPxVQw1EaA6JofVb034NkRbfV3kzgp+78GtJ8rqjRFhF9R95xcuz3wvg75VoONPYRW+R0rPsu5ehoOUcV67ewhIs387m/4sSCJbuQLjc1KJRfNBmTSRQM/pnQHgOrL5PLlooFKSejhOAeyJsr7v5levitoCzwli2OW0XbmjIq7xJgGNKmBDPZXHanxylx5GM+DA4cxcFEuG1Xn8Ia+1v0U+v6lOfXkOXDaqzZwil0EEQBwN3JXN10xJ5z/Xm8XlGcuJTDdsgonj09R919PR2nu4J8/3jBpCmXckhzwDikfRIQBagq8yRmpwGVxsPk1uhnYQreF1/J0ohI6eTQ==</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "<P>yTy+UFxG/4mozLxCFPVf3PjfhUiJad/UakyC/o+52GEgjGYRzFptvdPmJhC5NIM0DMxp7Jo1hlkS4FY64ob9+n+PSqe8iDAQI8GYi7Lod2tT+po456FkNGyYHas6CYNvy/1je5SKPZIvKMqxqdI3PicIbz1rsbmWPwxZ2qP+yLc=</P>" +
            "<Q>7dnngeZUdxfNhB6HERGqwktpB49vFA38A5zqajaEF/Cx6nadCC8L0VQVKRh5q9TZ7z6RoURf+BAwo091LtuC2j0WoFF6N4b40NXp3auljqRY/Wn0J0Ez3JAHu0Y25bhxrbcb7tGd+u+BYAmYP0hZXxSzV8JvGJjrM8jgZXi7JRs=</Q>" +
            "<DP>Ux7yBUN4WAjGXnrMWNfZlu59dRI3IDJCDP45hWhcfVHxHczzQROBDM2/3K0/sr8W+spcXkx8G1gjgeJcREE6GwQhbdbG+wHPSNWoatulRd5hO7wEETkKqNTKKEcXm+JchUfOlbcPOsttC05eACVCiDubMkeOekSMGNLDMppWnRU=</DP>" +
            "<DQ>GGzKE1Ld1aUCG9EuuZgA2xb3YClglMSi3pQclm+mvPagrp0jnxWFtydVTPUQeY7QGntoZREqccTn2b1tITS1iUfArRHw5QbdQkTVUUgxJibGZK+evPE7LCk2QgsB1DFOqzIkki/Ac2Zo+L3DfqwlUVhE2nxDAFmjvZuhP4pYXYU=</DQ>" +
            "<InverseQ>rURApxTU8ZQqKqHCjppcgwR01yDkecGgvMZ9d09OqnQ1cW7CWklPYQo2uC8plrb/Uw4M4R2lC0naWRmyUpayI8yWbdfSi56B9SnIhPQQBlTRw7esRFVIvAC9z/wv11Hg2sXEAjfvfbA3KX1O2zcQ+Ye92Dp+d00H6irCUuOA9bA=</InverseQ>" +
            "<D>IYzXHTm95HT8UN1fkept8jsbfyFlCWn/1LktbfhjCfDvhcDIm24V6ZHPZIv0Or+8vdIMPG+aZVCXVJRRgfG6gV1uZvVlllAOcj950rfXnatXBSqaV2L/Mm6sYeTtFq9vfRkAgFUE4T/v6N6RoObBDsIUalDxvwi/2Ebm5eTSKL8ILGC2mjeHc6oHRJNTonSaZxLS4BsdaIqcgjS5UCKGPBaHYkF7bZ3Dx6N4276iZdXMNP+B7p4mv5rUlXJi8e8UpNUoHRCV258rBZGh98ldVR0jmcEjvrf5N2XP78ataYglk6cSSBTghS0tWr1be/02eE+KsCzMwzjHWBn3eipAQQ==</D>" +
            "</RSAKeyValue>";

        public SecurityProvider() {
            rsa = RSA.Create();
            //use rsa key in xml to keep keys valid until they expire even if the server shuts down
            rsa.FromXmlString(rsaKeyXml);
        }

        public string GetToken(List<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();

            var credentials = new SigningCredentials(new RsaSecurityKey(rsa.ExportParameters(true)), SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken("www.webprogramming.com", "www.bethel.edu", claims,
                DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(15)), DateTime.UtcNow.AddDays(1), credentials);
            //Who gave out the token, who is supposed to use it - this can be anything you want
            //Claims, When the token is created, when the token is expired

            return handler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var validationParameters = new TokenValidationParameters()
            {
                ValidIssuer = "www.webprogramming.com",
                ValidAudience = "www.bethel.edu",
                IssuerSigningKey = new RsaSecurityKey(rsa.ExportParameters(false)),

            };

            var handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(token, validationParameters, out var validatedToken);

            } 
            catch
            {
                return false;
            }
            return true;

        }
    }
}
