using System;
using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 78680;
            const string apiKey = "c1fb5d6d-a036-4d7d-a35a-9342154ab0bb";
            const string apiSecret = "cae51871-4926-499c-aa87-049e3fd8a0ee";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);

            Workspace workspace = new Workspace("C4 Final, Dominik Mendoza", "Sismos");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model;
            // 1. Context Diagram
            SoftwareSystem enlazador = model.AddSoftwareSystem("SASpe", "Sistema de Alerta de Sismos peruano SASpe");
            SoftwareSystem sismate = model.AddSoftwareSystem("SISMATE", "Permite el envío de sms de alerta celular");
            SoftwareSystem coen = model.AddSoftwareSystem("COEN", "Centro de Operaciones de Emergencia Nacional");
            SoftwareSystem indeci = model.AddSoftwareSystem("INDECI", "Permite analizar y organizar el tratamiento de riesgo de siniestro");
            
            Person opRegional = model.AddPerson("Operador Regional", "Operador Regional");
            Person opNacional = model.AddPerson("Operador Nacional", "Operador Nacional");
            Person usuario = model.AddPerson("Usuario", "Usuario de la aplicación");
            


            usuario.Uses(enlazador, "Revisa los reportes de sismos y las alertas de tsunami dispuestas");
            opNacional.Uses(enlazador, "Envía alertas de tsunami");
            opRegional.Uses(enlazador, "Envía alertas de sismos");

            enlazador.Uses(indeci, "Envía reportes de sismos");
            enlazador.Uses(sismate, "Envía alertas de sismos");
            enlazador.Uses(coen, "Envía alertas de tsunami");

            // Tags
            
            //Administrador.AddTags("Administrador");
            enlazador.AddTags("SASpe");
            sismate.AddTags("SISMATE");
            coen.AddTags("COEN");
            indeci.AddTags("INDECI");
            usuario.AddTags("Usuario");
            opRegional.AddTags("Operador Regional");
            opNacional.AddTags("Operador Nacional");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#ffffff" });
            
            styles.Add((new ElementStyle("Usuario") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Person }));
            styles.Add((new ElementStyle("Operador Regional") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Person }));
            styles.Add((new ElementStyle("Operador Nacional") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Person }));
            styles.Add((new ElementStyle("SASpe") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.RoundedBox }));
            styles.Add((new ElementStyle("SISMATE") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.RoundedBox }));
            styles.Add((new ElementStyle("COEN") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.RoundedBox }));
            styles.Add((new ElementStyle("INDECI") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.RoundedBox }));

            SystemContextView contextView = viewSet.CreateSystemContextView(enlazador, "Context", "Context Diagram");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
            
            // 2. Container Diagram
            Container webApplication = enlazador.AddContainer("Web Application", "Permite al usuario revisar los reportes de sismos y las alertas de tsunami dispuestas", "ASP.NET Core MVC");
            Container mobileApplication = enlazador.AddContainer("Mobile Application", "Permite al usuario revisar los reportes de sismos y las alertas de tsunami dispuestas", "Xamarin");
            Container apiRest = enlazador.AddContainer("API Rest", "Permite al usuario revisar los reportes de sismos y las alertas de tsunami dispuestas", "ASP.NET Core Web API");
            Container igpLima = enlazador.AddContainer("IGP Lima", "Se encarga de recolectar los mensajes", "ASP.NET Core Web API");
            
            Container earthquakeContext = enlazador.AddContainer("Earthquake Context", "Bounded Context de deteccion de sismos", "INDECI");
            Container messageContext= enlazador.AddContainer("Message Processor", "Bounded Context donde se procesa el envio de mensajes", "INDECI");
            
            Container database = enlazador.AddContainer("Database", "Almacena los datos de los reportes de sismos y las alertas de tsunami dispuestas", "MongoDB");
            
            usuario.Uses(webApplication, "Revisa los reportes de sismos y las alertas de tsunami dispuestas");
            usuario.Uses(mobileApplication, "Revisa los reportes de sismos y las alertas de tsunami dispuestas");
            
            
            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            apiRest.Uses(database, "Reads from and writes to", "MongoDB");
            apiRest.Uses(earthquakeContext, "Envía alertas de sismos");
            apiRest.Uses(messageContext, "Envía alertas de sismos");
            
            earthquakeContext.Uses(database, "Reads from and writes to", "MongoDB");
            messageContext.Uses(database, "Reads from and writes to", "MongoDB");
            
            messageContext.Uses(sismate, "Envía alertas de sismos", "SMS");
            earthquakeContext.Uses(indeci, "Envía informacion para analizar y organizar el tratamiento de riesgo de siniestro", "Dedicated Internet line");
            
            // Tags
            webApplication.AddTags("Web Application");
            mobileApplication.AddTags("Mobile Application");
            apiRest.AddTags("API Rest");
            earthquakeContext.AddTags("Earthquake Context");
            messageContext.AddTags("Message Processor");
            database.AddTags("Database");
            string contextTag = "Context";
            styles.Add((new ElementStyle("Web Application") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.WebBrowser}));
            styles.Add((new ElementStyle("Mobile Application") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.MobileDevicePortrait}));
            styles.Add((new ElementStyle("API Rest") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.RoundedBox }));
            styles.Add((new ElementStyle("Earthquake Context") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Hexagon}));
            styles.Add((new ElementStyle("Message Processor") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Hexagon}));
            styles.Add((new ElementStyle("Database") { Background = "#438dd5", Color = "#ffffff", Shape = Shape.Cylinder}));
            styles.Add(new ElementStyle(contextTag) { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            
            
            ContainerView containerView = viewSet.CreateContainerView(enlazador, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();
            
            /* 3. Component Diagram
            Component webApplicationController = webApplication.AddComponent("Web Application Controller", "Maneja las solicitudes HTTP", "ASP.NET Core MVC");
            Component webApplicationView = webApplication.AddComponent("Web Application View", "Maneja las solicitudes HTTP", "ASP.NET Core MVC");
            Component webApplicationModel = webApplication.AddComponent("Web Application Model", "Maneja las solicitudes HTTP", "ASP.NET Core MVC");
            
            /* 
            // 3.1. Diagrama de Componentes (Account Context)
            Component accountController = AccountContext.AddComponent("Account Controller", "Controlador de Cuentas de Usuario.", "NodeJS (NestJS) REST Controller");
           
            Component accountCreator = AccountContext.AddComponent("Account Creator", "permite crear cuentas de usuario, pertenece a la capa Application de DDD", "NestJS Component");
            Component accountLogger = AccountContext.AddComponent("Account Logger", "permite validar los datos de login de un usuario, pertenece a la capa Application de DDD", "NestJS Component");
            
            Component accountRepository = AccountContext.AddComponent("Account Repository", "Concede el acceso a la base de datos de cuentas del usuario", "NestJS Component");
            Component accountMapper = AccountContext.AddComponent("Account Mapper", "Mapea los usuarios guardados en la base de datos", "NestJS Component");
            
            Component tempAccount = AccountContext.AddComponent("Temporal Logged Account", "Almacena los datos temporales del usuario en el que se ha iniciado sesión", "NestJS Component");
            Component facadeAccounts = AccountContext.AddComponent("Facade Account", "Sistema que permite registrarse o iniciar sesión con cuentas de otras plataformas", "NestJS Component");


            apiRest.Uses(accountController, "", "JSON/HTTPS");

            accountController.Uses(accountCreator, "Usa", "");
            accountController.Uses(accountLogger, "Usa", "");

            accountCreator.Uses(accountRepository, "Usa", "");
            accountCreator.Uses(facadeAccounts, "Usa", "");
            accountLogger.Uses(tempAccount, "Usa", "");
            accountLogger.Uses(accountRepository, "Usa", "");
            accountLogger.Uses(facadeAccounts, "Usa", "");
            accountRepository.Uses(accountMapper, "Solicita que lea o realice cambios a la base de datos", "");

            accountMapper.Uses(database, "Escribe y Lee datos de usuario", "");


            facadeAccounts.Uses(GoogleAccount, "Llama al API", "JSON/HTTPS");

            // Tags
            accountController.AddTags("Controller");
            accountCreator.AddTags("Service");
            accountLogger.AddTags("Service");
            accountRepository.AddTags("Repository");
            accountMapper.AddTags("Repository");
            tempAccount.AddTags("Entity");
            facadeAccounts.AddTags("Facade");
            AccountContext.AddTags("Account");



            styles.Add(new ElementStyle("Controller") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Service") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Account") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Repository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Entity") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("Facade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentViewAccount = viewSet.CreateComponentView(AccountContext, "AccountComponents", "Component Diagram");
            componentViewAccount.PaperSize = PaperSize.A4_Landscape;
            componentViewAccount.Add(mobileApplication);
            componentViewAccount.Add(apiRest);
            componentViewAccount.Add(database);
            componentViewAccount.Add(GoogleAccount);

            componentViewAccount.AddAllComponents();
            // 3.2. Diagrama de Componentes (Chatbot Context)
            Component chatbotController = ChatBotContext.AddComponent("Chatbot Controller", "Controlador de Chatbot, pertenece a la capa Application de DDD", "NodeJS (NestJS) REST Controller");

            Component mentalHealthIllnessRepository = ChatBotContext.AddComponent("Mental Health Illness Repository", "Permite al chatbot leer la información de distintos tipos de enfermedades mentales", "NestJS Component") ?? throw new ArgumentNullException("ChatBotContext.AddComponent(\"Mental Health Illness Repository\", \"Permite al chatbot leer la información de distintos tipos de enfermedades mentales\", \"NestJS Component\")");
            Component mentalHealthIllness = ChatBotContext.AddComponent("Mental Health Illness", "Almacena la enfermedad mental que el chatbot ha detectado", "NestJS Component");
            
            Component chatFarcade = ChatBotContext.AddComponent("Facade Chatbot", "Permite que los usuarios interactuen con un chatbot real, y que este indique al sistema que accion desea realizar el usuario, o que información desea visualizar", "NestJS Component");


            apiRest.Uses(chatbotController, "", "JSON/HTTPS");

            chatbotController.Uses(chatFarcade, "Usa", "");
            chatbotController.Uses(mentalHealthIllness, "Usa", "");
            chatbotController.Uses(mentalHealthIllnessRepository, "Usa", "");



            mentalHealthIllnessRepository.Uses(database, "Lee información sobre enfermedades mentales de la base de datos", "");
            mentalHealthIllness.Uses(database, "Asocia un paciente a una enfermedad mental", "");


            chatFarcade.Uses(ChatBot, "Llama al API", "JSON/HTTPS");

            // Tags
            chatbotController.AddTags("ControllerChatbot");
            mentalHealthIllnessRepository.AddTags("MentalRepository");
            mentalHealthIllness.AddTags("MentalHealthIllness");
            chatFarcade.AddTags("FacadeChat");


            styles.Add(new ElementStyle("MentalHealthIllness") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FacadeChat") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ControllerChatbot") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });;

            ComponentView componentViewChatbot = viewSet.CreateComponentView(ChatBotContext, "ChatbotComponents", "Component Diagram");
            componentViewChatbot.PaperSize = PaperSize.A4_Landscape;
            componentViewChatbot.Add(mobileApplication);
            componentViewChatbot.Add(apiRest);
            componentViewChatbot.Add(database);
            componentViewChatbot.Add(ChatBot);

            componentViewChatbot.AddAllComponents();


            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);

            // 3.3. Diagrama de Componentes (Payment Context)
            Component domainLayer = PaymentContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component paymentController = PaymentContext.AddComponent("Payment Controller", "REST API de pago de citas.", "NodeJS (NestJS) REST Controller");
            Component paymentApplicationService = PaymentContext.AddComponent("Dates PaymentService", "Provee métodos para el pago de citas, pertenece a la capa Application de DDD", "NestJS Component");
            Component paymentRepository = PaymentContext.AddComponent("Payment Repository", "Cuentas del usuario", "NestJS Component");
            Component paymentEntity = PaymentContext.AddComponent("Payment Entity", "Datos de los metodos de pago", "NestJS Component");
            Component facadePayment = PaymentContext.AddComponent("Facade Payment", "Sistema que maneja el pago para distintas plataformas", "NestJS Component");


            apiRest.Uses(paymentController, "", "JSON/HTTPS");
            paymentController.Uses(paymentApplicationService, "Invoca métodos de monitoreo");

            paymentApplicationService.Uses(domainLayer, "Usa", "");
            paymentApplicationService.Uses(paymentEntity, "Usa", "");
            paymentApplicationService.Uses(paymentRepository, "Usa", "");
            paymentApplicationService.Uses(facadePayment, "Usa", "");

            paymentRepository.Uses(database, "Usa", "");


            facadePayment.Uses(Yape, "Makes API calls to", "JSON/HTTPS");
            facadePayment.Uses(Plin, "Makes API calls to", "JSON/HTTPS");
            facadePayment.Uses(Tunki, "Makes API calls to", "JSON/HTTPS");
            facadePayment.Uses(Visa, "Makes API calls to", "JSON/HTTPS");

            // Tags
            domainLayer.AddTags("DomainLayer");
            paymentController.AddTags("PaymentController");
            paymentApplicationService.AddTags("PaymentApplicationService");
            paymentRepository.AddTags("PaymentRepository");
            paymentEntity.AddTags("PaymentEntity");
            facadePayment.AddTags("FacadePayment");

            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentEntity") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FacadePayment") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentViewPayment = viewSet.CreateComponentView(PaymentContext, "PaymentComponents", "Component Diagram");
            componentViewPayment.PaperSize = PaperSize.A4_Landscape;
            componentViewPayment.Add(mobileApplication);
            componentViewPayment.Add(apiRest);
            componentViewPayment.Add(database);
            componentViewPayment.Add(Visa);
            componentViewPayment.Add(Yape);
            componentViewPayment.Add(Tunki);
            componentViewPayment.Add(Plin);

            componentViewPayment.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);

            // 3.4. Diagrama de Componentes (Appointment Context)

            Component appointmentController = AppointmentContext.AddComponent("AppointmentController", "REST API endpoints de gestión de citas.", "NodeJS (NestJS) REST Controller");
            Component appointmentApplicationService = AppointmentContext.AddComponent("AppontmentApplicationService", "Provee métodos para la gestión de citas, pertenece a la capa Application de DDD", "NestJS Component");
            Component domainLayerAppointment = AppointmentContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component videoCallSystem = AppointmentContext.AddComponent("VideoCallSystem", "", "Component: NestJS Component");
            Component scheduleRepository = AppointmentContext.AddComponent("ScheduleRepository", "Horarios del psicólogo", "Component: NestJS Component");
            Component reviewRepository = AppointmentContext.AddComponent("ReviewRepository", "Comentarios a los psicológos", "Component: NestJS Component");
            Component appointmentDateRepository = AppointmentContext.AddComponent("AppointmentDateRepository", "Fechas de citas", "Component: NestJS Component");

            apiRest.Uses(appointmentController, "", "JSON/HTTPS");
            appointmentController.Uses(appointmentApplicationService, "Invoca métodos de gestión de citas");

            appointmentApplicationService.Uses(domainLayerAppointment, "Usa", "");
            appointmentApplicationService.Uses(videoCallSystem, "", "");
            appointmentApplicationService.Uses(scheduleRepository, "", "");
            appointmentApplicationService.Uses(reviewRepository, "", "");
            appointmentApplicationService.Uses(appointmentDateRepository, "", "");

            scheduleRepository.Uses(database, "", "");
            reviewRepository.Uses(database, "", "");
            appointmentDateRepository.Uses(database, "", "");

            videoCallSystem.Uses(GoogleMeet, "JSON/HTTPS");
            scheduleRepository.Uses(GoogleCalendar, "JSON/HTTPS");

            // Tags
            domainLayerAppointment.AddTags("DomainLayerAppointment");
            appointmentController.AddTags("AppointmentController");
            appointmentApplicationService.AddTags("AppointmentApplicationService");
            videoCallSystem.AddTags("VideoCallSystem");
            scheduleRepository.AddTags("SceduleRepository");
            reviewRepository.AddTags("ReviewRepository");
            appointmentDateRepository.AddTags("AppointmentDateRepository");

            styles.Add(new ElementStyle("DomainLayerAppointment") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VideoCallSystem") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SceduleRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ReviewRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AppointmentDateRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });


            ComponentView componentAppointmentView = viewSet.CreateComponentView(AppointmentContext, "AppointmentComponent", "Component Diagram");
            componentAppointmentView.PaperSize = PaperSize.A4_Landscape;

            componentAppointmentView.Add(mobileApplication);
            componentAppointmentView.Add(apiRest);
            componentAppointmentView.Add(database);
            componentAppointmentView.Add(GoogleMeet);
            componentAppointmentView.Add(GoogleCalendar);

            componentAppointmentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);

            //3.5. Diagrama de Componentes (Groups Context)
            Component domainLayerGroupsContext = GroupsContext.AddComponent("Domain Layer Groups", "", "NodeJS (NestJS)");
            Component mentalCareGroupsController = GroupsContext.AddComponent("Mental Care Groups Controller", "REST API de mental care groups.", "NodeJS (NestJS) REST Controller");
            Component mentalCareGroupsService = GroupsContext.AddComponent("Mental Care Groups Service", "Provee métodos para la realización de grupos de pacientes para su interacción, pertenece a la capa Application de DDD", "NestJS Component");
            Component mentalCareGroupsMeetingFacade = GroupsContext.AddComponent("Meeting System Facade", "Provee un sistema de tipo facade para reuniones en grupo", "NestJS Component");
            Component accountsRepository = GroupsContext.AddComponent("Accounts Repository", "Cuentas de usuarios de la aplicación", "NestJS Component");
            Component messagesRepository = GroupsContext.AddComponent("Messages Repository", "Mensajes de conversaciones entre los participantes de la aplicación", "NestJS Component");
            Component motivacionEmocionalRepository = GroupsContext.AddComponent("Motivación Emocial Repository", "Metodos de apoyo emocional a los pacientes", "NestJS Component");

            apiRest.Uses(mentalCareGroupsController, "", "JSON/HTTPS");
            mentalCareGroupsController.Uses(mentalCareGroupsService, "Invoca métodos de grupos de cuidado mental");
            mentalCareGroupsService.Uses(domainLayerGroupsContext, "Usa", "");
            mentalCareGroupsService.Uses(mentalCareGroupsMeetingFacade, "", "");
            mentalCareGroupsService.Uses(accountsRepository, "", "");
            mentalCareGroupsMeetingFacade.Uses(GoogleMeet, "", "JSON/HTTPS");
            accountsRepository.Uses(GoogleAccount, "", "JSON/HTTPS");
            accountsRepository.Uses(database, "", "");
            mentalCareGroupsService.Uses(messagesRepository, "", "");
            messagesRepository.Uses(database, "", "");
            motivacionEmocionalRepository.Uses(database, "", "");
            mentalCareGroupsService.Uses(motivacionEmocionalRepository, "", "");

            // Tags
            domainLayerGroupsContext.AddTags("DomainLayerGroups");
            mentalCareGroupsController.AddTags("MentalCareGroupsController");
            mentalCareGroupsService.AddTags("MentalCareGroupsService");
            mentalCareGroupsMeetingFacade.AddTags("MentalCareGroupsMeetingFacade");
            accountsRepository.AddTags("AccountsRepository");
            messagesRepository.AddTags("MessagesRepository");
            motivacionEmocionalRepository.AddTags("MotivacionEmocionalRepository");

            styles.Add(new ElementStyle("DomainLayerGroups") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalCareGroupsController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalCareGroupsService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalCareGroupsMeetingFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AccountsRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MessagesRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MotivacionEmocionalRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });


            ComponentView componentViewGroup = viewSet.CreateComponentView(GroupsContext, "GroupComponent", "Component Diagram");
            componentViewGroup.PaperSize = PaperSize.A4_Landscape;
            componentViewGroup.Add(mobileApplication);
            componentViewGroup.Add(apiRest);
            componentViewGroup.Add(database);
            componentViewGroup.Add(GoogleAccount);
            componentViewGroup.Add(GoogleMeet);

            componentViewGroup.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);


            //3.6. Diagrama de Componentes (Mental Health Diagnostic Context)

            Component domainLayerDiagnosticContext = MentalHealthContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component mentalCareDiagnosticController = MentalHealthContext.AddComponent("Mental Care Diagnostic Controller", "REST API de mental care groups.", "NodeJS (NestJS) REST Controller");
            Component mentalCareDiagnosticService = MentalHealthContext.AddComponent("Mental Care Diagnostic Service", "Provee métodos para la realización de grupos de pacientes para su interacción, pertenece a la capa Application de DDD", "NestJS Component");
            Component diagnosticRepository = MentalHealthContext.AddComponent("Accounts Repository", "Cuentas de usuarios de la aplicación", "NestJS Component");

            apiRest.Uses(mentalCareDiagnosticController, "", "JSON/HTTPS");

            mentalCareDiagnosticController.Uses(mentalCareDiagnosticService, "Invoca métodos para el control de diagnósticos");

            mentalCareDiagnosticService.Uses(domainLayerDiagnosticContext, "Usa", "");
            mentalCareDiagnosticService.Uses(ChatBot, "Usa", "");
            mentalCareDiagnosticService.Uses(diagnosticRepository, "", "");
            diagnosticRepository.Uses(database, "", "");
            mentalCareDiagnosticService.Uses(diagnosticRepository, "", "");
            ChatBot.Uses(database, "", "");


            //// Tags
            domainLayerDiagnosticContext.AddTags("DomainLayerDiagnostic");
            mentalCareDiagnosticController.AddTags("MentalCareDiagnosticController");
            mentalCareDiagnosticService.AddTags("MentalCareDiagnosticService");
            diagnosticRepository.AddTags("DiagnosticRepository");

            styles.Add(new ElementStyle("DomainLayerDiagnostic") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalCareDiagnosticController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MentalCareDiagnosticService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DiagnosticRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentViewDiagnostic = viewSet.CreateComponentView(MentalHealthContext, "MentalDiagnosticComponent", "Component Diagram");
            componentViewDiagnostic.PaperSize = PaperSize.A4_Landscape;
            componentViewDiagnostic.Add(mobileApplication);
            componentViewDiagnostic.Add(apiRest);
            componentViewDiagnostic.Add(database);
            componentViewDiagnostic.Add(ChatBot);

            componentViewDiagnostic.AddAllComponents();
            */

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}