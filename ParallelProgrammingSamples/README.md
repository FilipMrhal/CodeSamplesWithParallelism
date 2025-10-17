# Poznámky k async/await a paralelnímu programování:

Nejspolehlivější zdroj informací: https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/async-scenarios

1. Performance:
   1. Async/await umožňuje efektivněji využívat Thread Pool, takže v případě správného použití snižuje pravděpodobnost TP exhaustion.
   2. Async/await potřebuje větší množství alokované paměti, především kvůli stavovým strojům a taky musí nakonec výsledek mateody zabalit do IAwaitable (Tasku).
      1. Dá se tomu pomoci použitím ValueTask místo Task.
   3. V případě použití async/await v kritických často se opakujících voláních způsobíte GC pressure, případně memory leak.
   4. **Doporučuje se používat async/await u všech operací navázaných na I/O operace.** Důvod: Schopnost vykonávat I/O operace bez "dohledu" našeho programu je základní vlastní každého operačního systému. Dotnet vám umožní kvůli tomu neblokovat aplikaci.
2. Deadlocky:
   1. Nejčastěji se dějí u AsyncMethod().Result nebo AsyncMethod().Wait().
      1. Jestli nastane deadlock nebo ne závisí typicky na tom, jaký synchronizační kontext se použije.
      2. U desktopových aplikací nebo aplikací se silným konceptem hlavního vlákna typicky deadlock nastane.
      3. U ASP.NET CORE deadlock typicky nenastane. Ale pokud to použijete na nějaké kritické cestě, tak může dojít k TP Exhaustion.
3. Paralelní programování:
   1. AsParallel() v PLINQ je pomalé a nedoporučuji používat. V PLINQ je to proto, že F# má pro paralelní zpracování kolekcí mnohem lepší implementaci, ale sdílejí runtime. V C# to asi působilo jako low-hanging fruit.
   2. Parallel.ForEach atd je dobré a spolehlivé řešení. Pozor ale na scope v ASP.NET CORE. Třeba s DbContextem to nefunguje dobře.
   3. Pokud potřebuju algoritmus napsat paralelně, tak si to ideálně najdu v knize nebo vymyslím a vyzkouším za všech možných okolností.
   4. Pokud si nejsem jistý, jestli jsem kód vylepšil, můžu použít DotNetBenchmark knihovnu https://github.com/dotnet/BenchmarkDotNet. Případně funguje dobře https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-monitor. (Použitelný i na běžící aplikaci třeba na produkci. Nejčastěji se v Kubernetes řeší nasazením "sidecar" kontejneru.) 

## Použití nástroje dotnet-dump pro řešení deadlocku:
1. Instalace: https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-dump
2. Vysvětlivky pro unmanaged části kódu, které občas v dumpu uvidíte: https://learn.microsoft.com/en-us/dotnet/core/unmanaged-api/debugging/
3. Pro deadlocky typicky použijeme sekvenci příkazů: 
   1. `dotnet-dump collect -p <PID>`
   2. `dotnet-dump analyze <PID>`
   3. `threads`  --> Velký počet threadů je podezřelý i  z důvodu možné TP exhaustion, doporučuju zkontrolovat.
   4. `clrstack -all` --> Hledáme opakující se patterny v call stacku jednotlivých threadů. O deadlocku nás může informovat i nezvykle nebo nečekaně velký počet threadů.
   5. Podíváme se na tabulku zámků: `syncblk`
   6. Zbývá ověřit, které z nich jsou vzájemně blokující, to se dá udělat kontrolou call stacků daných threadů:
      ```
      setthread -t <threadid> 
      clrstack
      ```
      
## Vytvoření diagnostického dumpu je možné i automaticky:
1. Nastavíme proměnnou prostředí (env var): 
    ```
    DOTNET_DbgEnableMiniDump=1
    ```
   1. Pozor na Docker a Kubernetes - zápis proběhne automaticky do /tmp. Když se kontejner recykluje, tak soubor zmizí. 
   2. Někdy je i problém s právy, typicky nutné řešit s DevOps. x