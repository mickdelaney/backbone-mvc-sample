var _App = _App || {};

UserSession = Backbone.Model.extend({
    initialize: function (options) {
        this.bind('change', function () {
            console.log('UserSession: values for this model have changed');
            //this.collection.trigger("itemChanged", this);
        });
        this.bind("error", function (model, error) {
            console.log('UserSession: error: ' + error);
            //this.collection.trigger("itemError", error);
        });
    },
    idAttribute: 'Id',
    url: function () {
        if(this.isNew()) {
            return "/Accounts/usersessions";
        } else {
            return "/Accounts/usersessions/" + this.get('Id');
        }
    },
    validate: function (attrs) {
        if (attrs.Name == '') {
            return "Must provide a name";
        }
    }
});

UserSessionCollection = Backbone.Collection.extend({
    model: UserSession,        
    url: "/Accounts/usersessions"                
});

UserSessionListItemView = Backbone.View.extend({
    tagName: "li",
    className: "usersession group",
    events: {
        "click": "editSession"
    },
    initialize: function (options) {
        _.bindAll(this, "render");

        //Pub/sub
        this.eventAg = options.eventAg;

        //UI
        var source = $("#item-template");
        this.template = Handlebars.compile(source.html());
    },
    editSession: function (e) {
        e.preventDefault();

        editForm = new EditUserSessionView({ model: this.model, eventAg: this.eventAg });
        editForm.render();
        return false;
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

        var self = this;
        this.eventAg = options.eventAg;
        this.collection.bind("add", function (model) {
            self.render();
        });

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
        this.eventAg = options.eventAg;
        this.form = $(this.el);
        this.model = new UserSession();
    },
    //events on this element (el)            
    events: {
        "change input": "updateModel",
        "submit": "save"
    },
    updateModel: function (evt) {
        var field = $(evt.currentTarget);
        var data = {};
        var key = field.attr('NAME');
        var val = field.val();
        data[key] = val;
        if (!this.model.set(data)) {
            //reset the form field
            field.val(this.model.get(key));
        }
    },
    //save (http post)
    save: function (e) {
        e.preventDefault();

        var self = this;
        this.model.save(this.model.attributes, {
                success: function (model, response) {
                    console.log('created session. publishing event. model:' + JSON.stringify(model));
                    self.eventAg.trigger("userSession:Created", model);
                },
                error: function (model, response) {
                    console.log('error creating session. error:' + JSON.stringify(response));
                }
            }
        );
    }
});

EditUserSessionView = Backbone.View.extend({
    el: "#edit-session-form",
    initialize: function (options) {
        _.bindAll(this, "render");
        
        this.eventAg = options.eventAg;
    },
    //events on the el            
    events: {
        "change input": "updateModel",
        "submit": "save"
    },
    updateModel: function (evt) {
        var field = $(evt.currentTarget);
        var data = {};
        var key = field.attr('NAME');
        var val = field.val();
        data[key] = val;
        if (!this.model.set(data)) {
            //reset the form field
            field.val(this.model.get(key));
        }
    },
    render: function () {
        $("#edit-name").val(this.model.get('Name'));
    },
    save: function (e) {
        e.preventDefault();

        var self = this;
        this.model.save(this.model.attributes, {
                success: function (model, response) {
                    self.eventAg.trigger("userSession:Updated", model);
                }
            }
        );
    }
});

UserSessionsPage = Backbone.Router.extend({
    initialize: function (options) {
        _.bindAll(this, "start", "edit", "userSessionCreated");

        //Setup pub/sub
        this.eventAg = _.extend({}, Backbone.Events);
        this.eventAg.bind("userSession:Created", this.userSessionCreated);

        //Data
        this.sessions = new UserSessionCollection();

        //UI
        //set the collection on the list view. it will then pick up any changes.
        this.list = new UserSessionListView({ eventAg: this.eventAg, collection: this.sessions });

        //this view will bind to a form and add new items to the collection + publish events
        this.createView = new CreateUserSessionView({ eventAg: this.eventAg });
    },
    routes: {
        "": "start",
        "edit/:id": "edit"
    },
    edit: function (id) {
        if (this.sessions && this.sessions.length > 0) {
            var session = this.sessions.get(id);
            if (session) {
                var editView = new EditUserSessionView({ Session: session });
                editView.render();
            }
        }
        else {
            //re-fetch the sessions - Bug!! need to fix.
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
        console.log('UserSessionsPage.userSessionCreated: handling userSessionCreated created: model: ' + JSON.stringify(userSession));

        //Add the data
        this.sessions.add(userSession);
    },
    start: function () {
        console.log('UserSessionsPage.start: starting...');

        //handle scope of this.
        var self = this;
        this.sessions.fetch({
            success: function () {
                self.list.render();
            }
        });

    }
});