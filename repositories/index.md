<h3>List of Public Repo's </h3>

{% for repository in site.github.public_repositories %}
{{ repository.full_name }} - {{repository.description}}
{% endfor %}