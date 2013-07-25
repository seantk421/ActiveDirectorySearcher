ActiveDirectorySearcher
=======================

Use the ADSearch class to search the specified Active Directory computer and user objects.  The results will be 
returned as either a list of ADComputers or a list of ADUsers.  When instantiating the searcher object, you will 
need to specify the domain you wish to connect to and the username and password of a basic user account in the 
domain.  Below is an example of retrieving a dataset and binding to a simple gridview.

```

ASSearcher searcher = new ADSearcher("mycompanydomain.com", "username", "password");
List<ADUser> users = searcher.SearchUserDescription("searchquery");

GridViewName.DataSource = users;
GridViewName.DataBind();

```

Easy as that. :)

You can expand on the search results, Active Directory has numerious Computer/User properties which can be returned in
a search query.



