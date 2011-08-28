var _App = _App || {};

UserSession = Backbone.Model.extend({
    initialize: function (options) {
    },
    idAttribute: 'Id',    
    url: function () {
        return "/Accounts/usersessions";
    }
//    ,validate: function (attrs) {
//        if (!attrs.Name) {
//            return "Must provide a name";
//        }
//    }
});

UserSessionCollection = Backbone.Collection.extend({
    model: UserSession,        
    url: "/Accounts/usersessions"                
});

UserSessionListItemView = Backbone.View.extend({
    tagName: "li",
    initialize: function (options) {
        _.bindAll(this, "render");

        //Pub/sub
        this.eventAg = options.eventAg;
        
        //UI
        var source = $("#item-template");
        this.template = Handlebars.compile(source.html());
    },
    render: function () {
        var content = this.template(this.model.toJSON());
        $(this.el).html(content);
        return this;
    }
});

UserSessionListView = Backbone.View.extend({
    el: '#sessions-list',
    initialize: function (options) {
        _.bindAll(this, "render");
        
        this.eventAg = options.eventAg;
    },
    render: function () {
        var self = this;
        var list = $(this.el);
        list.empty();
        var els = [];

        this.collection.each(function (model) {
            var view = new UserSessionListItemView({ model: model });
            els.push(view.render().el);
        });
        list.append(els);

        return this;
    }
});

CreateUserSessionView = Backbone.View.extend({
    el: "#create-session-form",
    initialize: function (options) {
        _.bindAll(this, "save");
        
        this.eventAg = options.eventAg;
        this.form = $(this.el);
    },
    //events on the el            
    events: {
        "submit": "save"
    },
    save: function (e) {
        e.preventDefault();

        var self = this;
        var name = $("#create-name").val();
        var userSession = new UserSession();
        userSession.set({ Name: name });
        userSession.save({
            success: function (model, response) {
                self.eventAg.trigger("userSession:Created", model);
            }
        });
    }
});

EditUserSessionView = Backbone.View.extend({
    el: "#edit-session-form",
    initialize: function (options) {
        _.bindAll(this, "save", "render");

        this.Session = options.session;
        this.eventAg = options.eventAg;
        this.render();
    },
    //events on the el            
    events: {
        "submit": "save"
    },
    render: function () {
        $("#edit-name").val(this.Session.get('Name'));
    },
    save: function (e) {
        e.preventDefault();

        var self = this;
        var name = $("#edit-name").val();

        this.Session.set({ Name: name });
        this.Session.save({
            success: function (model, response) {
                self.eventAg.trigger("userSession:Updated", model);
            }
        });
    }
});

UserSessionsPage = Backbone.Router.extend({
    initialize: function (options) {
        _.bindAll(this, "start", "edit");

        //Setup pub/sub
        this.eventAg = _.extend({}, Backbone.Events);
        this.eventAg.bind("userSession:Created", this.userSessionCreated);

        //Data
        this.sessions = new UserSessionCollection();

        //UI
        this.list = new UserSessionListView({ eventAg: this.eventAg });
        this.createView = new CreateUserSessionView({ eventAg: this.eventAg });
    },
    routes: {
        "": "start",
        "edit/:id": "edit"
    },
    edit: function (id) {
        if (this.sessions) {
            var session = this.sessions.get(id);
            var editView = new EditUserSessionView({ Session: session });
            editView.render();
        }
        else {

            var self = this;
            this.sessions.fetch({
                success: function () {
                    self.list.collection = self.sessions;
                    self.list.render();

                    var session = self.sessions.get(id);
                    var editView = new EditUserSessionView({ Session: session });
                    editView.render();
                }
            });

        }
    },
    userSessionCreated: function (userSession) {
        //Add the data
        this.sessions.add(userSession);

        //Re-render the list
        this.list.collection = sessions;
        this.list.render();
    },
    start: function () {

        var self = this;
        this.sessions.fetch({
            success: function () {
                self.list.collection = self.sessions;
                self.list.render();
            }
        });

    }
});