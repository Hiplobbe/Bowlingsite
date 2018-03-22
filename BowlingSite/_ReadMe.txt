Tankar och funderingar
	Jätte kul uppgift, inte ännu haft chans att sitta i .net Core så det vad väldigt kul att få testa på, men en hel del av tiden 
	spenderad gick åt för TypeScript(som jag började med),och att få controllern att konvertera indatat(Så här i efterhand borde jag kanske inte antagit att 
	det hanteras exakt som i vanliga .Net behövde bara lägga till [FromBody]).

	Unittesten ändrade en massa i applikationen, till det bättre. Och kommer hädanefter använda dessa betydligt mer!

"Missing features"
	En typ av restart? 
	Krav speccen uttrycker rätt klart att "Player starts a new game by visiting the web site:" och förfrågar ingen restart funktion,
	i nuläget kan man paja systemet genom att ladda om sidan och då sabba flödet, detta är medvetet och lösningen hade varit att kontrollern 
	tar emot rund siffran eller rent utav ignorerar den.

	För få test/Lite css? 
	För lite fantasi är boven för detta.

	Fler scoring/feltolkning av reglerna?
	Ja detta händer, som jag skrivit i kommentarer är detta medvetet och ett försök att följa speccen så nära som möjligt, 
	men ändå ge en bra bowling upplevelse.

	Validering av data?
	Försökt så gott jag kunnat men har medvetet skippat vissa bitar, främst för att egentligen fanns det inga krav på anti fusk, 
	men det kändes naturligt att ha till viss del.