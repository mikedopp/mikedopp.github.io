List of Public Repo's

{% for repository in site.github.public_repositories %}
{{ repository.full_name }} - {{repository.description}}
{% endfor %}