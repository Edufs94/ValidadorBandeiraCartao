using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public abstract class Cartao
{
    public abstract string Nome { get; }
    public abstract Regex Regex { get; }
}

public class Visa : Cartao
{
    public override string Nome => "Visa";
    public override Regex Regex => new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$|^4[0-9]{15}$");
}

public class AmericanExpress : Cartao
{
    public override string Nome => "American Express";
    public override Regex Regex => new Regex(@"^3[47][0-9]{13}$");
}

public class Discover : Cartao
{
    public override string Nome => "Discover";
    public override Regex Regex => new Regex(@"^6(?:011|5[0-9]{2}|4[4-9][0-9])[0-9]{12}$");
}

public class Hipercard : Cartao
{
    public override string Nome => "Hipercard";
    public override Regex Regex => new Regex(@"^606282|^3841(?:[0|4|6]{1})0");
}

public class Mastercard : Cartao
{
    public override string Nome => "Mastercard";
    public override Regex Regex => new Regex(@"^5[1-5][0-9]{14}$|^2(2[2-9][1-9]|[3-6][0-9]{2}|7[01][0-9]|720)[0-9]{12}$");
}

public class Elo : Cartao
{
    public override string Nome => "Elo";
    public override Regex Regex => new Regex(@"^(4011|4312|4389)[0-9]{12}$");
}

public class DinersClub : Cartao
{
    public override string Nome => "Diners Club";
    public override Regex Regex => new Regex(@"^3(?:0[0-5]|[68][0-9])[0-9]{11}$");
}

public class EnRoute : Cartao
{
    public override string Nome => "enRoute";
    public override Regex Regex => new Regex(@"^2(?:014|149)[0-9]{11}$");
}

public class JCB : Cartao
{
    public override string Nome => "JCB";
    public override Regex Regex => new Regex(@"^(?:352[89]|35[3-8][0-9])[0-9]{12,15}$");
}

public class Voyager : Cartao
{
    public override string Nome => "Voyager";
    public override Regex Regex => new Regex(@"^8699[0-9]{11}$");
}

public class Aura : Cartao
{
    public override string Nome => "Aura";
    public override Regex Regex => new Regex(@"^50[0-9]{17}$");
}

public class ValidadorCartao
{
    private static readonly List<Cartao> Cartoes = new List<Cartao>
    {
        new Visa(),
        new AmericanExpress(),
        new Discover(),
        new Hipercard(),
        new Mastercard(),
        new Elo(),
        new DinersClub(),
        new EnRoute(),
        new JCB(),
        new Voyager(),
        new Aura()
    };

    public static string ValidarBandeira(string numeroCartao)
    {
        numeroCartao = numeroCartao.Replace(" ", ""); 

        if (!Regex.IsMatch(numeroCartao, @"^\d+$"))
        {
            return "Número de cartão inválido";
        }

        if (!ValidarLuhn(numeroCartao))
        {
            return "Número de cartão inválido";
        }

        foreach (var cartao in Cartoes)
        {
            if (cartao.Regex.IsMatch(numeroCartao))
            {
                return cartao.Nome;
            }
        }

        return "Bandeira desconhecida";
    }

    private static bool ValidarLuhn(string numeroCartao)
    {
        int soma = 0;
        bool alternar = false;

        for (int i = numeroCartao.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(numeroCartao[i].ToString());

            if (alternar)
            {
                n *= 2;
                if (n > 9)
                {
                    n -= 9;
                }
            }

            soma += n;
            alternar = !alternar;
        }

        return (soma % 10 == 0);
    }
}

public class Program
{
    public static void Main()
    {
        bool continuar = true;

        while (continuar)
        {
            Console.WriteLine("Digite o número do cartão:");
            string? numeroCartao = Console.ReadLine();

            if (string.IsNullOrEmpty(numeroCartao))
            {
                Console.WriteLine("Número de cartão inválido");
            }
            else
            {
                string bandeira = ValidadorCartao.ValidarBandeira(numeroCartao);
                Console.WriteLine($"A bandeira do cartão é: {bandeira}");
            }

            Console.WriteLine("Deseja validar outro cartão? (S/N)");
            string? resposta = Console.ReadLine();

            if (resposta == null || resposta.ToUpper() != "S")
            {
                continuar = false;
            }
        }
    }
}