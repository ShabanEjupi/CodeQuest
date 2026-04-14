using CodeQuest.Models;

namespace CodeQuest.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        var existingKeys = db.Chapters.Select(c => c.Language + "_" + c.GameType + "_" + c.Concept).ToHashSet();
        var chapters = new List<Chapter>();

        // EN & SQ helper to create bilingual chapters easily
        void AddChapter(int order, string concept, 
            string enLabel, string enStory, string enCode, string enPrompt, string enOk, string enBad,
            string sqLabel, string sqStory, string sqCode, string sqPrompt, string sqOk, string sqBad,
            (string text, bool ok)[] enChoices,
            (string text, bool ok)[] sqChoices,
            string gameType = "Coding")
        {
            if (!existingKeys.Contains("en_" + gameType + "_" + concept))
            {
                var cEn = new Chapter { Language = "en", GameType = gameType, OrderIndex = order, Concept = concept, Label = enLabel, StoryHtml = enStory, CodeHtml = enCode, QuizPrompt = enPrompt, OkFeedback = enOk, BadFeedback = enBad };
                for (int i=0; i<enChoices.Length; i++) cEn.Choices.Add(new Choice { Text = enChoices[i].text, IsCorrect = enChoices[i].ok, OrderIndex = i });
                chapters.Add(cEn);
            }
            else
            {
                var existingEn = db.Chapters.FirstOrDefault(c => c.Language == "en" && c.GameType == gameType && c.Concept == concept);
                if (existingEn != null) { existingEn.Label = enLabel; existingEn.StoryHtml = enStory; existingEn.CodeHtml = enCode; existingEn.QuizPrompt = enPrompt; existingEn.OkFeedback = enOk; existingEn.BadFeedback = enBad; existingEn.OrderIndex = order; }
            }

            if (!existingKeys.Contains("sq_" + gameType + "_" + concept))
            {
                var cSq = new Chapter { Language = "sq", GameType = gameType, OrderIndex = order, Concept = concept, Label = sqLabel, StoryHtml = sqStory, CodeHtml = sqCode, QuizPrompt = sqPrompt, OkFeedback = sqOk, BadFeedback = sqBad };
                for (int i=0; i<sqChoices.Length; i++) cSq.Choices.Add(new Choice { Text = sqChoices[i].text, IsCorrect = sqChoices[i].ok, OrderIndex = i });
                chapters.Add(cSq);
            }
            else
            {
                var existingSq = db.Chapters.FirstOrDefault(c => c.Language == "sq" && c.GameType == gameType && c.Concept == concept);
                if (existingSq != null) { existingSq.Label = sqLabel; existingSq.StoryHtml = sqStory; existingSq.CodeHtml = sqCode; existingSq.QuizPrompt = sqPrompt; existingSq.OkFeedback = sqOk; existingSq.BadFeedback = sqBad; existingSq.OrderIndex = order; }
            }
        }

        // 1. VARIABLES
        AddChapter(0, "Variables",
            "Chapter 1 · The POS Terminal",
            "<p>You connect to the <strong>KOSOVAPOS Mainframe</strong>...</p><p>A variable is like a <strong>labelled box</strong> — you give it a name, and put a value inside it.</p>",
            "<span class='kw'>cashier_name</span> = <span class='str'>\"Arben\"</span>",
            "The POS asks: \"What IS a variable?\"",
            "✓ Exactly right! You spoke the name — 'cashier_name' — and the terminal unlocked.",
            "✗ A variable is like a labelled box.",
            "Kapitulli 1 · Terminali i KOSOVAPOS",
            "<p>Lidheni me <strong>Sistemin KOSOVAPOS</strong>...</p><p>Një variabël është si një <strong>kuti me etiketë</strong> — i jepni një emër dhe futni një vlerë brenda.</p>",
            "<span class='kw'>emri_arkëtarit</span> = <span class='str'>\"Agroni\"</span>",
            "Sistemi i mbrojtjes së arkës aktivizohet dhe të shtron një pyetje sfiduese: \"A mund të më thuash, çfarë ËSHTË saktësisht një variabël në botën e programimit?\"",
            "✓ Saktë! Përdorët emrin dhearka u zhbllokua.",
            "✗ Një variabël është si një kuti me etiketë.",
            new[] { ("A labelled container that stores a value", true), ("A type of loop that repeats forever", false), ("A system command", false), ("A constantly changing number", false) },
            new[] { ("Një enë e emërtuar që ruan një vlerë", true), ("Një lloj unaze që përsëritet përgjithmonë", false), ("Një komandë e veçantë", false), ("Një numër i pandryshueshëm", false) });

        // 2. DATA TYPES
        AddChapter(1, "Data Types",
            "Chapter 2 · The Order Processor",
            "<p>Order details drift on your screen... The main types: int, string, boolean.</p>",
            "<span class='kw'>is_pizza_ready</span> = <span class='kw'>True</span>",
            "A fragment reads: \"is_oven_hot = False\". What data type is this?",
            "✓ Correct! Booleans only hold True or False.",
            "✗ 'False' is a boolean value.",
            "Kapitulli 2 · Procesori i Porosive",
            "<p>Porositë shfaqen në ekran... Llojet kryesore: int, string, boolean.</p>",
            "<span class='kw'>eshte_pica_gati</span> = <span class='kw'>True</span>",
            "Një nga udhëzimet në ekran tregon: \"eshte_furra_ngrohte = False\". Duke marrë parasysh atë që sapo mësove, cili lloj i të dhënave mund të mbajë këtë vlerë?",
            "✓ E saktë! Boolean mund të jetë vetëm True ose False.",
            "✗ 'False' është një vlerë boolean.",
            new[] { ("int", false), ("string", false), ("boolean", true), ("float", false) },
            new[] { ("int (numër i plotë)", false), ("string (tekst)", false), ("boolean (E vërtetë/E gabuar)", true), ("float (numër me presje)", false) });

        // 3. CONDITIONALS
        AddChapter(2, "Conditionals",
            "Chapter 3 · The Logic Bridge",
            "<p>Two paths lead forward... The bridge chooses based on a <strong>condition</strong>. If True, left. Else, right.</p>",
            "<span class='kw'>if</span> has_flashlight:\n    <span class='fn'>print</span>(<span class='str'>\"Walk the lit path.\"</span>)\n<span class='kw'>else</span>:\n    <span class='fn'>print</span>(<span class='str'>\"Darkness.\"</span>)",
            "If has_flashlight is False, which line prints?",
            "✓ Right! When condition is False, 'else' block runs.",
            "✗ When condition is False, only 'else' runs.",
            "Kapitulli 3 · Ura e Logjikës",
            "<p>Dy rrugë hapen... Ura zgjedh bazuar në një <strong>kusht (condition)</strong>. Nëse është True (e vërtetë), shko majtas. Përndryshe (Else), djathtas.</p>",
            "<span class='kw'>if</span> ka_drite:\n    <span class='fn'>print</span>(<span class='str'>\"Rruga e ndriçuar.\"</span>)\n<span class='kw'>else</span>:\n    <span class='fn'>print</span>(<span class='str'>\"Errësirë.\"</span>)",
            "Kujdes, kjo është e rëndësishme: Nëse variabla 'ka_drite' kthehet si False (E gabuar), cilin rresht kodi do të shohësh të printuar në ekranin tënd?",
            "✓ Saktë! Kur kushti është False, blloku 'else' ekzekutohet.",
            "✗ Kur kushti është False, ekzekutohet vetëm 'else'.",
            new[] { ("Both lines", false), ("Walk the lit path.", false), ("Darkness.", true), ("Neither", false) },
            new[] { ("Të dy rreshtat", false), ("Rruga e ndriçuar.", false), ("Errësirë.", true), ("Asnjëri", false) });

        // 4. LOOPS
        AddChapter(3, "Loops",
            "Chapter 4 · The Repeating Process",
            "<p>There are 100 files to extract. We use a <strong>loop</strong> to repeat an action for every item.</p>",
            "<span class='kw'>for</span> <span class='var'>file</span> <span class='kw'>in</span> files:\n    <span class='fn'>extract</span>(<span class='var'>file</span>)",
            "The list has 4 files. How many times does extract() run?",
            "✓ Exactly! The loop runs 4 times.",
            "✗ It runs once per item.",
            "Kapitulli 4 · Procesi Përsëritës",
            "<p>Ka 100 skedarë për t'u hapur. Ne përdorim një <strong>Unazë (Loop)</strong> për të përsëritur një veprim për secilin element.</p>",
            "<span class='kw'>for</span> <span class='var'>dosje</span> <span class='kw'>in</span> dosjet:\n    <span class='fn'>hap</span>(<span class='var'>dosje</span>)",
            "Supozojmë që kemi një listë me saktësisht 4 skedarë. Duke përdorur këtë unazë (loop), sa herë do të thirret funksioni per t'i hapur?",
            "✓ Pikërisht! Unaza ekzekutohet 4 herë.",
            "✗ Ajo ekzekutohet një herë për secilin element.",
            new[] { ("1 time", false), ("4 times", true), ("Infinite times", false), ("0 times", false) },
            new[] { ("1 herë", false), ("4 herë", true), ("Pafundësisht", false), ("0 herë", false) });

        // 5. FUNCTIONS
        AddChapter(4, "Functions",
            "Chapter 5 · The Core Components",
            "<p>We must repeat a sequence of steps 3 times. We bundle them into a <strong>function</strong>—a reusable package of code.</p>",
            "<span class='kw'>def</span> <span class='fn'>process_data</span>(<span class='var'>name</span>): ...",
            "What is the MAIN benefit of wrapping code in a function?",
            "✓ Perfect! You define it once and reuse it.",
            "✗ Reusability is the main point.",
            "Kapitulli 5 · Komponentët Baze",
            "<p>Ne duhet të përsërisim disa hapa 3 herë. Ne i grupojmë ato në një <strong>funksion (function)</strong>—një paketë kodi e ripërdorshme.</p>",
            "<span class='kw'>def</span> <span class='fn'>proceso_te_dhenat</span>(<span class='var'>emri</span>): ...",
            "Ndërsa vazhdon të eksplorosh, duhet të kuptosh thelbin: Cila është arsyeja KRYESORE dhe më e rëndësishme pse ne i grupojmë komandat tona brenda një funksioni?",
            "✓ Shkëlqyeshëm! E shkruan një herë dhe e përdor shumë herë.",
            "✗ Ripërdorimi i kodit është qëllimi kryesor.",
            new[] { ("Makes code run faster", false), ("Define once, reuse many times", true), ("Replaces variables", false), ("Only for numbers", false) },
            new[] { ("E bën kodin të ekzekutohet më shpejt", false), ("E shkruan kodin një herë, e përdor shumë herë", true), ("Zëvendëson variablat", false), ("Punon vetëm me numra", false) });

        // 6. ARRAYS / LISTS (NEW CHAPTER)
        AddChapter(5, "Lists / Arrays",
            "Chapter 6 · The Infinite Database",
            "<p>You find a massive system directory. To manage so many records, you need a <strong>List (or Array)</strong>. A List holds multiple items in a specific order within one single variable.</p>",
            "<span class='kw'>records</span> = [<span class='str'>\"User_Data\"</span>, <span class='str'>\"System_Log\"</span>]",
            "How do you organize multiple items together in order?",
            "✓ Correct! A list holds them together.",
            "✗ A list or array manages ordered items.",
            "Kapitulli 6 · Baza e të Dhënave e Pafundme",
            "<p>Gjeni një direktori të madhe sistemi. Për të menaxhuar gjithë këto regjistrime, nevojitet një <strong>Listë/Matricë (List/Array)</strong>. Një listë mban shumë elementë njëri pas tjetrit në një variabël të vetme.</p>",
            "<span class='kw'>regjistrat</span> = [<span class='str'>\"Te_Dhenat\"</span>, <span class='str'>\"Log_Sistemi\"</span>]",
            "Tani që keni gjithë këtë sasi informacioni, si mund t'i ruani dhe organizoni të gjithë këta elementë në një rregull të caktuar brenda kompjuterit?",
            "✓ E saktë! Një listë i mban së bashku.",
            "✗ Një listë menaxhon elementë në seri.",
            new[] { ("With an integer", false), ("Using a Loop", false), ("In a List / Array", true), ("With an if-statement", false) },
            new[] { ("Me anë të një numri të plotë", false), ("Duke përdorur një Unazë (Loop)", false), ("Në një Listë (List/Array)", true), ("Ato krijohen automatikisht", false) });

        // 7. DICTIONARIES
        AddChapter(6, "Dictionaries",
            "Chapter 7 · The Index of Nodes",
            "<p>To find the exact key for each server, the system uses a <strong>Dictionary (Map)</strong>. Instead of numbered positions, Dictionaries link a \"Key\" (like a server name) to a \"Value\" (like its passcode).</p>",
            "<span class='kw'>passcodes</span> = {<span class='str'>\"ServerA\"</span>: <span class='str'>\"1234\"</span>, <span class='str'>\"ServerB\"</span>: <span class='str'>\"5678\"</span>}",
            "What makes a Dictionary different from a simple List?",
            "✓ Spot on! Keys and Values link data directly.",
            "✗ Dictionaries use Key-Value pairs.",
            "Kapitulli 7 · Treguesi i Nyjeve",
            "<p>Për të gjetur kodin e saktë të secilit server, sistemi përdor një <strong>Fjalor (Dictionary)</strong>. Fjalorët lidhin një \"Çelës\" (si emri i serverit) me një \"Vlerë\" (si fjalëkalimi i saj).</p>",
            "<span class='kw'>koded</span> = {<span class='str'>\"ServeriA\"</span>: <span class='str'>\"1234\"</span>, <span class='str'>\"ServeriB\"</span>: <span class='str'>\"5678\"</span>}",
            "Sistemi kërkon saktësi absolute: Cila është karakteristika themelore që e bën një Fjalor (Dictionary) të ndryshëm dhe më të përshtatshëm sesa një Listë e thjeshtë për raste të veçanta?",
            "✓ Perfekte! Çelësat dhe vlerat e strukturojnë atë.",
            "✗ Fjalorët përdorin çifte Çelës-Vlerë (Key-Value).",
            new[] { ("It loops forever", false), ("It pairs Keys and Values", true), ("It only stores numbers", false), ("It automatically compresses data", false) },
            new[] { ("Përsërit përgjithmonë", false), ("Çifton një Çelës me një Vlerë", true), ("Ruan vetëm numra", false), ("Kompjeshon të dhënat", false) });

        // 8. CLASSES
        AddChapter(7, "Classes",
            "Chapter 8 · The Blueprints",
            "<p>A Class acts like a blueprint for creating objects. It defines properties and behaviors that its objects will have in the exact same format.</p>",
            "<span class='kw'>class</span> Robot:\n    <span class='kw'>def</span> <span class='fn'>__init__</span>(self, name):\n        self.name = name",
            "What is the primary role of a Class in programming?",
            "✓ Exactly! A class is a blueprint for objects.",
            "✗ A class acts as a blueprint.",
            "Kapitulli 8 · Skicat Teknologjike",
            "<p>Një Klasë (Class) shërben si një skicë për krijimin e objekteve. Ajo përcakton vetitë dhe sjelljet që do të kenë objektet e ngjashme.</p>",
            "<span class='kw'>class</span> Roboti:\n    <span class='kw'>def</span> <span class='fn'>__init__</span>(self, emri):\n        self.emri = emri",
            "Ndërsa vazhdojmë të ndërtojmë sisteme komplekse, duhet të kuptosh se cili është roli kryesor i një Klase në programim?",
            "✓ Përkryer! Klasa është një skicë për objektet.",
            "✗ Një klasë funksionon si skicë për objektet.",
            new[] { ("To define a blueprint for real objects", true), ("To style text", false), ("To stop the program", false), ("To multiply variables", false) },
            new[] { ("Të ofrojë një skicë për objektet", true), ("Të formatojë tekstin", false), ("Të ndalojë programin", false), ("Të shumëzojë variablat", false) });

        // 9. EXCEPTIONS
        AddChapter(8, "Exceptions",
            "Chapter 9 · Error Handling",
            "<p>When unexpected issues happen, they crash the app. We use <strong>Try-Except (Catch)</strong> mechanisms to gracefully handle exceptions and keep the system online.</p>",
            "<span class='kw'>try</span>:\n    <span class='fn'>connect</span>()\n<span class='kw'>except</span> Exception:\n    <span class='fn'>print</span>(<span class='str'>\"Connection Failed!\"</span>)",
            "What is the main purpose of an Exception handler?",
            "✓ Correct! It catches errors safely.",
            "✗ Exception handlers deal with unexpected errors.",
            "Kapitulli 9 · Menaxhimi i Gabimeve",
            "<p>Kur ndodhin probleme të papritura, programi mbyllet papritur. Përdorim blloqet <strong>Try-Except (Catch)</strong> për të menaxhuar gabimet pa prishur punën e sistemit.</p>",
            "<span class='kw'>try</span>:\n    <span class='fn'>lidhu</span>()\n<span class='kw'>except</span> Exception:\n    <span class='fn'>print</span>(<span class='str'>\"Lidhja Dështoi!\"</span>)",
            "Tashmë që kemi ardhur kaq larg, duhet të sigurojmë stabilitet: Cili është qëllimi kryesor i një blloku për menaxhimin e gabimeve (Exception handler)?",
            "✓ E saktë! Ai kap gabimet pa mbyllur sistemin.",
            "✗ Menaxhon gabimet dhe ruan programin nga bllokimi.",
            new[] { ("To make the system crash faster", false), ("To catch and manage unpredictable errors", true), ("To delete corrupted files by default", false), ("To ignore correct logic", false) },
            new[] { ("Të shkaktojë kolaps më shpejt", false), ("Të menaxhojë gabimet e papritura", true), ("Të fshijë dokumentet me problem", false), ("Të injorojë kodin e saktë", false) });

        // 10. ASYNC/AWAIT
        AddChapter(9, "Async/Await",
            "Chapter 10 · The Asynchronous Network",
            "<p>When a task takes a long time, like downloading a file, the system shouldn't freeze. <strong>Asynchrony (Async/Await)</strong> lets the program keep running while waiting for the task to finish.</p>",
            "<span class='kw'>async def</span> <span class='fn'>download_data</span>():\n    <span class='kw'>await</span> <span class='fn'>fetch_file</span>()",
            "Why do we use async/await?",
            "✓ Great! It prevents the system from freezing while waiting.",
            "✗ Async helps prevent blocking operations.",
            "Kapitulli 10 · Rrjeti Asinkron",
            "<p>Kur një detyrë zgjat shumë, si shkarkimi i një skedari, sistemi nuk duhet të ngrijë. <strong>Ekzekutimi Asinkron (Async/Await)</strong> lejon programin të vazhdojë ndërkohë që pret përfundimin e detyrës.</p>",
            "<span class='kw'>async def</span> <span class='fn'>shkarko_te_dhena</span>():\n    <span class='kw'>await</span> <span class='fn'>merr_skedar</span>()",
            "Pse përdorim programimin asinkron?",
            "✓ E saktë! Ai parandalon ngrirjen e sistemit.",
            "✗ Async shmang bllokimin e operacioneve.",
            new[] { ("To make code run slower", false), ("To stop the program", false), ("To prevent freezing while waiting", true), ("To create endless loops", false) },
            new[] { ("Të ngadalësojë kodin", false), ("Të ndalojë programin", false), ("Të parandalojë ngrirjen gjatë pritjes", true), ("Të krijojë unaza pa fund", false) });

        // 11. APIS
        AddChapter(10, "APIs",
            "Chapter 11 · The Communication Port",
            "<p>Systems talk to each other through <strong>APIs (Application Programming Interfaces)</strong>. They allow different programs to send and receive data securely.</p>",
            "<span class='var'>response</span> = <span class='fn'>requests.get</span>(<span class='str'>\"https://api.server.com/status\"</span>)",
            "What is the main role of an API?",
            "✓ Spot on! APIs enable communication between different systems.",
            "✗ APIs are used for communication between systems.",
            "Kapitulli 11 · Porta e Komunikimit",
            "<p>Sistemet komunikojnë me njëri-tjetrin nëpërmjet <strong>API-ve (Application Programming Interfaces)</strong>. Ato lejojnë programe të ndryshme të dërgojnë dhe marrin të dhëna të sigurta.</p>",
            "<span class='var'>pergjegja</span> = <span class='fn'>kerkesa.merr</span>(<span class='str'>\"https://api.server.com/status\"</span>)",
            "Cili është roli kryesor i një API-je?",
            "✓ E saktë! API-të mundësojnë komunikimin mes sistemeve.",
            "✗ API-të përdoren për të komunikuar mes programeve.",
            new[] { ("To design user interfaces", false), ("To allow programs to communicate", true), ("To delete system files", false), ("To draw 3D graphics", false) },
            new[] { ("Të dizenjojë pamjet për përdoruesit", false), ("Të lejojë programet të komunikojnë", true), ("Të fshijë dokumentet e sistemit", false), ("Të vizatojë grafika 3D", false) });

        // 12. DATABASES
        AddChapter(11, "Databases",
            "Chapter 12 · The Data Vault",
            "<p>To store data permanently, we use <strong>Databases</strong>. Using languages like SQL, we can save, retrieve, update, and delete millions of records efficiently.</p>",
            "<span class='kw'>SELECT</span> * <span class='kw'>FROM</span> Users <span class='kw'>WHERE</span> role = <span class='str'>'admin'</span>;",
            "What is the primary purpose of a Database?",
            "✓ Correct! It stores data permanently and efficiently.",
            "✗ Databases are used for permanent data storage.",
            "Kapitulli 12 · Kasaforta e të Dhënave",
            "<p>Për të ruajtur të dhënat përgjithmonë, ne përdorim <strong>Bazat e të Dhënave (Databases)</strong>. Duke përdorur gjuhë si SQL, mund të ruajmë, gjejmë, përditësojmë dhe fshijmë miliona regjistrime në mënyrë efikase.</p>",
            "<span class='kw'>SELECT</span> * <span class='kw'>FROM</span> Perdoruesit <span class='kw'>WHERE</span> roli = <span class='str'>'admin'</span>;",
            "Cili është qëllimi kryesor i një Baze të Dhënash?",
            "✓ E saktë! Ruan të dhënat përgjithmonë dhe në mënyrë efikase.",
            "✗ Bazat e të dhënave përdoren për ruajtjen e përhershme.",
            new[] { ("To render web pages", false), ("To store and manage data permanently", true), ("To format text", false), ("To style the application", false) },
            new[] { ("Të shfaqë faqet e internetit", false), ("Të ruajë dhe menaxhojë të dhënat përgjithmonë", true), ("Të formatojë tekstin", false), ("Të stilojë aplikacionin", false) });

        // 13. VERSION CONTROL
        AddChapter(12, "Version Control",
            "Chapter 13 · The Time Machine",
            "<p>When building systems, you need a way to track changes. <strong>Version Control (like Git)</strong> saves history, letting you revert mistakes and merge work with others.</p>",
            "<span class='kw'>git</span> commit -m <span class='str'>\"Added new feature\"</span>",
            "Why is Version Control important?",
            "✓ Great! It tracks changes and helps collaboration.",
            "✗ Version control tracks your code's history.",
            "Kapitulli 13 · Makina e Kohës",
            "<p>Gjatë ndërtimit të sistemeve, duhet të gjurmoni ndryshimet. <strong>Kontrolli i Versioneve (si Git)</strong> ruan historikun, duke ju lejuar të ktheni gabimet mbrapsht dhe të bashkoni punën me të tjerët.</p>",
            "<span class='kw'>git</span> commit -m <span class='str'>\"Shtuar tipar i ri\"</span>",
            "Pse është i rëndësishëm Kontrolli i Versioneve?",
            "✓ Saktë! Gjurmon ndryshimet dhe ndihmon bashkëpunimin.",
            "✗ Kontrolli i versioneve ruan historinë e kodit.",
            new[] { ("To encrypt passwords", false), ("To run code faster", false), ("To track changes and collaborate", true), ("To compile code to machine language", false) },
            new[] { ("Të kriptojë fjalëkalimet", false), ("Të ekzekutojë kodin më shpejt", false), ("Të gjurmojë ndryshimet dhe të bashkëpunojë", true), ("Të përkthejë kodin", false) });

        // 14. INHERITANCE
        AddChapter(13, "Inheritance",
            "Chapter 14 · The Parent Protocols",
            "<p>Instead of rewriting code, classes can <strong>Inherit</strong> from parents, gaining all their attributes.</p>",
            "<span class='kw'>class</span> Admin(User):\n    <span class='fn'>pass</span>",
            "What does inheritance allow you to do?",
            "✓ Right! It lets you reuse an existing structure.",
            "✗ Think about reusing parent code.",
            "Kapitulli 14 · Protokollet Prindërore",
            "<p>Në vend që të rishkruajnë kod, klasat mund të <strong>Trashëgojnë (Inherit)</strong> nga prindërit, duke marrë të gjitha vetitë e tyre.</p>",
            "<span class='kw'>class</span> Admin(User):\n    <span class='fn'>pass</span>",
            "Çfarë ju lejon të bëni trashëgimia?",
            "✓ Saktë! Të ripërdorni një strukturë ekzistuese.",
            "✗ Mendoni për ripërdorimin e kodit prindër.",
            new[] { ("Run programs concurrently", false), ("Inherit and reuse code from another class", true), ("Delete old code automatically", false), ("Prevent variables from changing", false) },
            new[] { ("Ekzekutimin e programeve njëkohësisht", false), ("Trashëgimin dhe ripërdorimin e kodit nga një klasë tjetër", true), ("Fshirjen e kodit të vjetër automatikisht", false), ("Parandalimin e ndryshimit të variablave", false) });

        // 15. UNIT TESTING
        AddChapter(14, "Unit Testing",
            "Chapter 15 · The Safety Checks",
            "<p>To ensure our functions work, we write <strong>Unit Tests</strong>. These are separate pieces of code that run our actual code with test data.</p>",
            "<span class='kw'>def</span> <span class='fn'>test_add</span>():\n    <span class='kw'>assert</span> <span class='fn'>add</span>(2, 3) == 5",
            "Why do we write Unit Tests?",
            "✓ Excellent! Tests verify individual parts of the code.",
            "✗ Unit tests check if code works as expected.",
            "Kapitulli 15 · Kontrollet e Sigurisë",
            "<p>Për t'u siguruar që funksionet tona punojnë, shkruajmë <strong>Testime Njësie (Unit Tests)</strong>. Këto testojnë kodin tonë me të dhëna provë.</p>",
            "<span class='kw'>def</span> <span class='fn'>test_mbledhja</span>():\n    <span class='kw'>assert</span> <span class='fn'>mbledhja</span>(2, 3) == 5",
            "Pse shkruajmë Testime Njësie?",
            "✓ E shkëlqyer! Testet verifikojnë pjesët e veçanta të kodit.",
            "✗ Testimet e njësisë kontrollojnë nëse kodi punon siç pritet.",
            new[] { ("To test individual components of the code", true), ("To increase application size", false), ("To translate code to English", false), ("To style text on a webpage", false) },
            new[] { ("Për të testuar komponentë të veçantë të kodit", true), ("Për të rritur madhësinë e aplikacionit", false), ("Për të përkthyer kodin në anglisht", false), ("Për të stiluar tekstin", false) });

        // POS Chapters
        AddChapter(0, "IntroPOS",
            "Chapter 1 · The POS Interface",
            "<p>Welcome to <strong>KosovaPOS</strong>! You are starting your shift. First, what does POS stand for and why is it essential for any retail business?</p><img src='https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?auto=format&fit=crop&q=80&w=800' alt='POS Intro' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "--- Starting POS System ---",
            "What does POS stand for?",
            "✓ Correct! It is the Point of Sale.",
            "✗ Think about where a sale happens.",
            "Kapitulli 1 · Ndërfaqja e POS",
            "<p>Mirësevini në <strong>KosovaPOS</strong>! Po fillon ndërrimin tuaj. Së pari, çfarë do të thotë POS dhe pse është thelbësor?</p><img src='https://images.unsplash.com/photo-1556742049-0cfed4f6a45d?auto=format&fit=crop&q=80&w=800' alt='POS Intro' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "--- Nisja e POS ---",
            "Çfarë do të thotë POS?",
            "✓ Saktë! Do të thotë Pika e Shitjes.",
            "✗ Mendo ku ndodh shitja.",
            new[] { ("Point of Sale", true), ("Proof of System", false), ("Part of Store", false), ("Point of Service", false) },
            new[] { ("Pika e Shitjes (Point of Sale)", true), ("Provë e Sistemit", false), ("Pjesë e Dyqanit", false), ("Pikë Shërbimi", false) }, "POS");

        AddChapter(1, "FiscalPrinter",
            "Chapter 2 · The Fiscal Printer",
            "<p>In the Republic of Kosova, every sale must go through a <strong>Fiscal Printer</strong> (Arka Fiskale) certified by ATK (Administrata Tatimore e Kosovës).</p><img src='https://images.unsplash.com/photo-1518458028785-8fbcd101ebb9?auto=format&fit=crop&q=80&w=800' alt='Fiscal Printer' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "--- Fiscal Printer Connection: OK ---",
            "Why do we need a Fiscal Printer?",
            "✓ Yes! It ensures sales are reported and taxes are paid.",
            "✗ It's required by law for tax recording.",
            "Kapitulli 2 · Arka Fiskale",
            "<p>Në Republikën e Kosovës, çdo shitje duhet të kalojë përmes një <strong>Arke Fiskale</strong> të certifikuar nga ATK-ja.</p><img src='https://images.unsplash.com/photo-1518458028785-8fbcd101ebb9?auto=format&fit=crop&q=80&w=800' alt='Arka Fiskale' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "--- Lidhja me Arkën Fiskale: OK ---",
            "Pse përdorim Arkën Fiskale?",
            "✓ Saktë! Raporton shitjet tek ATK-ja.",
            "✗ Është e domosdoshme ligjërisht.",
            new[] { ("To record sales for the Tax Administration (ATK)", true), ("To print nicer receipts", false), ("To run the internet", false), ("To hold cash safely", false) },
            new[] { ("Për të raportuar shitjet tek Administrata Tatimore (ATK)", true), ("Për të printuar kuponë më të bukur", false), ("Për internet", false), ("Për të mbajtur paratë", false) }, "POS");

        AddChapter(2, "Barcode",
            "Chapter 3 · Barcodes & Products",
            "<p>Let's scan our first product! A barcode scanner reads the EAN/UPC code to find the exact item in our KosovaPOS database.</p><img src='https://images.unsplash.com/photo-1607344645866-009c520b61c9?auto=format&fit=crop&q=80&w=800' alt='Scanner' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "SCAN: 3830001234567 -> Found: 'Ujë Mineral 0.5L'",
            "What does the scanner actually scan?",
            "✓ Correct, the barcode translates into a unique ID.",
            "✗ It reads the barcode.",
            "Kapitulli 3 · Barkodet",
            "<p>Le të skanojmë produktin e parë! Skaneri lexon kodin EAN/UPC për të gjetur artikullin thelbësor.</p><img src='https://images.unsplash.com/photo-1607344645866-009c520b61c9?auto=format&fit=crop&q=80&w=800' alt='Scanner' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "SKANIM: 3830001234567 -> Gjetur: 'Ujë Mineral 0.5L'",
            "Çfarë lexon saktësisht skaneri?",
            "✓ Saktë, barkodi përfaqëson një ID unike.",
            "✗ Lexon barkodin e produktit.",
            new[] { ("The unique barcode (EAN/UPC)", true), ("The ingredients", false), ("The expiration date", false), ("The business tax number", false) },
            new[] { ("Barkodin unik (EAN/UPC)", true), ("Përbërësit", false), ("Datën e skadencës", false), ("Numrin fiskal të biznesit", false) }, "POS");

        AddChapter(3, "VAT",
            "Chapter 4 · Value Added Tax (VAT / TVSH)",
            "<p>In Kosovo, standard VAT (TVSH) is 18%, while for essential items and utilities it might be 8%. The POS calculates this automatically before printing the fiscal receipt.</p>",
            "Price: 10.00 EUR | TVSH (18%): 1.53 EUR",
            "What is the standard VAT (TVSH) rate in Kosovo for most items?",
            "✓ Spot on! 18% is the standard rate.",
            "✗ The standard rate is 18%.",
            "Kapitulli 4 · TVSH",
            "<p>Në Kosovë, TVSH-ja standarde është 18%, ndërsa për produkte esenciale 8%. Sistemi i llogarit këtë automatikisht.</p>",
            "Çmimi: 10.00 EUR | TVSH (18%): 1.53 EUR",
            "Sa është norma standarde e TVSH-së në Kosovë?",
            "✓ Saktë! 18% është norma standarde.",
            "✗ 18% është përgjigja korrekte.",
            new[] { ("18%", true), ("8%", false), ("20%", false), ("10%", false) },
            new[] { ("18%", true), ("8%", false), ("20%", false), ("10%", false) }, "POS");

        AddChapter(4, "PaymentMethods",
            "Chapter 5 · Payment Methods",
            "<p>A customer wants to pay for their order. You can accept Cash, Credit/Debit Card, or Mobile Payments. In the fiscal receipt, the payment method must be clearly marked.</p><img src='https://images.unsplash.com/photo-1556742044-fbbd63327d53?auto=format&fit=crop&q=80&w=800' alt='Payment' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "Total: 15.50 EUR. Pay with: [CASH] [CARD]",
            "Why does the payment type matter for the fiscal printer?",
            "✓ Yes, ATK requires tracking of cash vs. electronic payments.",
            "✗ It matters for accounting and tax declaring.",
            "Kapitulli 5 · Kthimi i Kusurit",
            "<p>Klienti po paguan. Mund të pranoni Para të Gatshme (Cash) ose Kartelë. Lloji i pagesës printohet theksueshëm në kuponin fiskal.</p><img src='https://images.unsplash.com/photo-1556742044-fbbd63327d53?auto=format&fit=crop&q=80&w=800' alt='Payment' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "Total: 15.50 EUR. Paguaj me: [CASH] [KARTELË]",
            "Pse duhet specifikuar metoda e pagesës në arkë fiskale?",
            "✓ Kështu Administrata Tatimore e di saktë si janë marrë paratë.",
            "✗ Është për raportim të saktë.",
            new[] { ("It must match the daily closing statement for taxes (Z-Report)", true), ("To change the product price", false), ("So the cash drawer opens slower", false), ("It does not matter", false) },
            new[] { ("Duhet të përputhet me mbylljen ditore për ATK-në (Raporti Z)", true), ("Për të ndryshuar çmimin", false), ("Që të hapet arka më ngadalë", false), ("Nuk ka rëndësi", false) }, "POS");

        AddChapter(5, "ZReport",
            "Chapter 6 · The Z-Report",
            "<p>At the end of your shift, you must close the register. This prints a <strong>Z-Report</strong> (Raporti Z) from the fiscal printer, summarizing all sales, taxes, and money received.</p>",
            "--- PRINTING Z-REPORT ---",
            "What is a Z-Report?",
            "✓ Correct! It is the mandatory daily closure report.",
            "✗ The Z-Report is for daily closing.",
            "Kapitulli 6 · Raporti Z",
            "<p>Në fund të ndërrimit tuaj për ditën e punës, duhet mbyllur arka. Kjo printon një <strong>Raport Z</strong> nga arka fiskale, që tregon të gjitha shitjet, TVSH-të dhe paratë e gatshme.</p>",
            "--- PRINTIMI I RAPORTIT Z ---",
            "Çfarë është Raporti Z?",
            "✓ Saktesisht. Është mbyllja obligative ditore.",
            "✗ Është raporti i mbylljes ditore.",
            new[] { ("A daily financial summary required by ATK to close the registry", true), ("A report of only returned items", false), ("Inventory breakdown", false), ("A maintenance ticket", false) },
            new[] { ("Një përmbledhje ditore financiare që kërkohet nga ATK", true), ("Një raport vetëm për kthimet", false), ("Lista e inventarit", false), ("Një biletë mirëmbajtjeje", false) }, "POS");

        AddChapter(6, "Returns",
            "Chapter 7 · Handling Returns (Storno)",
            "<p>Sometimes customers return products. In POS, you perform a Return or Storno. For fiscal reasons, the original receipt must be matched.</p>",
            "ACTION: Storno | Receipt #1045",
            "What must you typically do when a customer returns a purchased item in Kosovo?",
            "✓ Yes! You issue a specific return fiscal receipt or record the storno officially.",
            "✗ You must register it in the POS to balance the books.",
            "Kapitulli 7 · Kthimet (Storno)",
            "<p>Ndonjëherë klientët kthejnë produkte. Në sistemin POS, bëhet Storno. Për rregulla fiskale, duhet të keni kuponin e origjinës.</p>",
            "VEPRIM: Storno | Kuponi #1045",
            "Çfarë bëhet gjatë kthimit të produktit me para?",
            "✓ Saktë! Duhet të lëshohet kupon kthimi dhe të përditësohet stoku.",
            "✗ Duhet regjistruar kthimin zyrtarisht në KosovaPOS.",
            new[] { ("Process it through POS to print a return fiscal receipt & update stock", true), ("Trash the original receipt and give hidden cash back", false), ("Nothing, returns aren't allowed", false), ("Ignore VAT changes", false) },
            new[] { ("Kalohet në POS për kupon të kthimit & përditësim të depose/stokut", true), ("Hidhni kuponin dhe kthe paratë fshehurazi", false), ("Asgjë, nuk lejohen", false), ("Injorohenndryshimet e TVSH-së", false) }, "POS");

        AddChapter(7, "Inventory",
            "Chapter 8 · Inventory Management",
            "<p>Stock levels must be accurate. Every time a sale is completed, the items are mechanically deducted from the inventory. When a truck arrives, you add stock.</p><img src='https://images.unsplash.com/photo-1586528116311-ad8ed74509b5?auto=format&fit=crop&q=80&w=800' alt='Inventory' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "INVENTORY UPDATE LOG",
            "What happens to the inventory when a sale is finalized?",
            "✓ Correct! Quantities are updated automatically.",
            "✗ Think about stock accuracy.",
            "Kapitulli 8 · Menaxhimi i Stokut",
            "<p>Nivelet e stokut duhet të jenë të sakta. Sa herë një shitje mbyllet, artikujt zbriten nga inventari. Kur vjen një dërgesë, shtohet stoku.</p><img src='https://images.unsplash.com/photo-1586528116311-ad8ed74509b5?auto=format&fit=crop&q=80&w=800' alt='Inventory' style='max-width:100%; object-fit:cover; border-radius:10px; margin: 15px 0;' />",
            "LOG I PËRDITËSIMIT TË STOKUT",
            "Çfarë i ndodh inventarit sapo kryhet një shitje?",
            "✓ Saktë! Sasitë përditësohen automatikisht.",
            "✗ Mendo për saktësinë e stokut.",
            new[] { ("Inventory levels are automatically deducted", true), ("Nothing happens", false), ("Inventory is manually written on paper", false), ("The shift ends", false) },
            new[] { ("Nivelet e inventarit zbriten automatikisht", true), ("Asgjë nuk ndodh", false), ("Inventari shkruhet manualisht në letër", false), ("Ndërrimi mbaron", false) }, "POS");

        AddChapter(8, "Discounts",
            "Chapter 9 · Discounts & Loyalty",
            "<p>Offering discounts or a Loyalty Card gives special perks to returning customers. Applying it in the POS adjusts the final price while calculating taxes correctly.</p>",
            "CUSTOMER: VIP | DISCOUNT: 10%",
            "Why is the discount processed through the POS system directly?",
            "✓ Exactly, to ensure proper tax calculation and transparent pricing.",
            "✗ Taxes must be accurate based on final price.",
            "Kapitulli 9 · Zbritjet & Besnikëria",
            "<p>Ofrimi i zbritjeve ose një Kartelë Besnikërie i jep përfitime të veçanta klientëve. Aplikimi në POS rregullon çmimin final duke llogaritur TVSH-në saktë.</p>",
            "KLIENT: VIP | ZBRITJE: 10%",
            "Pse zbritja duhet kaluar gjithmonë përmes sistemit POS?",
            "✓ Ashtu është, për të llogaritur TVSH-në saktë në çmimin final.",
            "✗ Taksat kërkojnë vlerën pas zbritjes.",
            new[] { ("To recalculate taxes on the new discounted price", true), ("To just make the receipt look longer", false), ("It doesn't have to be, you can give cash back", false), ("Only to collect emails", false) },
            new[] { ("Të rillogariten taksat në çmimin e ri me zbritje", true), ("Vetëm për të bërë kuponin të gjatë", false), ("S'ka nevojë, ktheni cash dore", false), ("Për të marrë emailin e klientit", false) }, "POS");

        AddChapter(9, "CashDrawer",
            "Chapter 10 · The Cash Drawer",
            "<p>The cash drawer contains the day's physical money and is secured. It pops open ONLY when an authorized sale is recorded or via a secure 'Open Drawer' command by managers.</p>",
            ">> DRAWER KICK SIGNAL SENT",
            "When should the cash drawer electronically open?",
            "✓ Correct, usually only upon closing a transaction.",
            "✗ Mostly it opens on finalized transactions.",
            "Kapitulli 10 · Sirtari i Parave",
            "<p>Sirtari mban paratë fizike dhe është i siguruar. Ai hapet VETËM kur një shitje e autorizuar regjistrohet ose nga një komandë e sigurt nga menaxherët.</p>",
            ">> KOMANDA PËR HAPJE E DËRGUAR",
            "Kur duhet të hapet elektronikisht sirtari i parave?",
            "✓ Saktë, sapo të mbyllet një transaksion valid.",
            "✗ Zakonisht hapet pas printimit të kuponit.",
            new[] { ("Only when a valid transaction is finalized or by manager key", true), ("Whenever a customer walks in", false), ("Every 5 minutes", false), ("It stays unlocked all day", false) },
            new[] { ("Vetëm pasi mbyllet transaksioni ose me çelës menaxheri", true), ("Sa herë hyn një klient", false), ("Çdo 5 minuta", false), ("Qëndron hapur gjithë ditën", false) }, "POS");

        if (chapters.Any())
        {
            db.Chapters.AddRange(chapters);
        }
        db.SaveChanges();

        // ── Seed KosovaPOS Business Data ──────────────────────────────────────
        if (!db.Businesses.Any())
        {
            var b1 = new Business { Name = "Supermarket Meridian", BusinessType = "Supermarket", Address = "Dardania, Prishtinë", Phone = "044111222", TaxNumber = "810123456" };
            var p1 = new PosSystem { Business = b1, Version = "3.1", SystemType = "Supermarket POS", FiscalPrinterEnabled = true, Theme = "Dark" };
            db.Businesses.Add(b1);
            db.PosSystems.Add(p1);

            var b2 = new Business { Name = "Pizzeria Proper", BusinessType = "Pizzeria", Address = "Ulpiana, Prishtinë", Phone = "045333444", TaxNumber = "810654321" };
            var p2 = new PosSystem { Business = b2, Version = "2.9", SystemType = "Pizzeria POS", FiscalPrinterEnabled = true, Theme = "Light" };
            db.Businesses.Add(b2);
            db.PosSystems.Add(p2);

            db.SaveChanges();
        }
    }
}
