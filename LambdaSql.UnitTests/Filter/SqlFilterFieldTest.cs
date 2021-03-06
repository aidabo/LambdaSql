﻿using LambdaSql.Field;
using LambdaSql.Filter;
using LambdaSql.UnitTests.Entities;
using Xunit;

namespace LambdaSql.UnitTests.Filter
{
    public class SqlFilterFieldTest
    {
        [Fact]
        public void SatisfyLambda()
        {
            var filter = SqlFilter<Person>.From(m => m.Id).SatisfyLambda(field => $"{field} IN (5)");
            Assert.Equal("pe.Id IN (5)", filter.RawSql);
        }

        [Fact]
        public void IsNull()
        {
            Assert.Equal("pe.Id IS NULL", SqlFilter<Person>.From(m => m.Id).IsNull().RawSql);
        }

        [Fact]
        public void IsNotNull()
        {
            Assert.Equal("pe.Name IS NOT NULL", SqlFilter<Person>.From(m => m.Name).IsNotNull().RawSql);
        }

        [Fact]
        public void Like()
        {
            Assert.Equal("pe.Name LIKE '%template'", SqlFilter<Person>.From(m => m.Name).Like("%template").RawSql);
        }

        [Fact]
        public void EqualTo()
        {
            Assert.Equal("pe.Id = 5", SqlFilter<Person>.From(m => m.Id).EqualTo(5).RawSql);
            Assert.Equal("pe.Name = pe.LastName", SqlFilter<Person>.From(m => m.Name).EqualTo(m => m.LastName).RawSql);
            Assert.Equal("pe.Id = pa.PersonId", SqlFilter<Person>.From(m => m.Id).EqualTo<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name = per.LastName", SqlFilter<Person>.From(m => m.Name).EqualTo(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id = pas.PersonId", SqlFilter<Person>.From(m => m.Id).EqualTo<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id = COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).EqualTo(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id = COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).EqualTo(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void NotEqualTo()
        {
            Assert.Equal("pe.Id <> 5", SqlFilter<Person>.From(m => m.Id).NotEqualTo(5).RawSql);
            Assert.Equal("pe.Name <> pe.LastName", SqlFilter<Person>.From(m => m.Name).NotEqualTo(m => m.LastName).RawSql);
            Assert.Equal("pe.Id <> pa.PersonId", SqlFilter<Person>.From(m => m.Id).NotEqualTo<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name <> per.LastName", SqlFilter<Person>.From(m => m.Name).NotEqualTo(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id <> pas.PersonId", SqlFilter<Person>.From(m => m.Id).NotEqualTo<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id <> COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).NotEqualTo(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id <> COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).NotEqualTo(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void GreaterThan()
        {
            Assert.Equal("pe.Id > 5", SqlFilter<Person>.From(m => m.Id).GreaterThan(5).RawSql);
            Assert.Equal("pe.Name > pe.LastName", SqlFilter<Person>.From(m => m.Name).GreaterThan(m => m.LastName).RawSql);
            Assert.Equal("pe.Id > pa.PersonId", SqlFilter<Person>.From(m => m.Id).GreaterThan<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name > per.LastName", SqlFilter<Person>.From(m => m.Name).GreaterThan(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id > pas.PersonId", SqlFilter<Person>.From(m => m.Id).GreaterThan<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id > COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).GreaterThan(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id > COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).GreaterThan(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void GreaterThanOrEqual()
        {
            Assert.Equal("pe.Id >= 5", SqlFilter<Person>.From(m => m.Id).GreaterThanOrEqual(5).RawSql);
            Assert.Equal("pe.Name >= pe.LastName", SqlFilter<Person>.From(m => m.Name).GreaterThanOrEqual(m => m.LastName).RawSql);
            Assert.Equal("pe.Id >= pa.PersonId", SqlFilter<Person>.From(m => m.Id).GreaterThanOrEqual<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name >= per.LastName", SqlFilter<Person>.From(m => m.Name).GreaterThanOrEqual(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id >= pas.PersonId", SqlFilter<Person>.From(m => m.Id).GreaterThanOrEqual<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id >= COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).GreaterThanOrEqual(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id >= COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).GreaterThanOrEqual(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void LessThan()
        {
            Assert.Equal("pe.Id < 5", SqlFilter<Person>.From(m => m.Id).LessThan(5).RawSql);
            Assert.Equal("pe.Name < pe.LastName", SqlFilter<Person>.From(m => m.Name).LessThan(m => m.LastName).RawSql);
            Assert.Equal("pe.Id < pa.PersonId", SqlFilter<Person>.From(m => m.Id).LessThan<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name < per.LastName", SqlFilter<Person>.From(m => m.Name).LessThan(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id < pas.PersonId", SqlFilter<Person>.From(m => m.Id).LessThan<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id < COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).LessThan(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id < COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).LessThan(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void LessThanOrEqual()
        {
            Assert.Equal("pe.Id <= 5", SqlFilter<Person>.From(m => m.Id).LessThanOrEqual(5).RawSql);
            Assert.Equal("pe.Name <= pe.LastName", SqlFilter<Person>.From(m => m.Name).LessThanOrEqual(m => m.LastName).RawSql);
            Assert.Equal("pe.Id <= pa.PersonId", SqlFilter<Person>.From(m => m.Id).LessThanOrEqual<Passport>(m => m.PersonId).RawSql);

            var peAlias = new SqlAlias<Person>("per");
            var paAlias = new SqlAlias<Passport>("pas");
            Assert.Equal("pe.Name <= per.LastName", SqlFilter<Person>.From(m => m.Name).LessThanOrEqual(m => m.LastName, peAlias).RawSql);
            Assert.Equal("pe.Id <= pas.PersonId", SqlFilter<Person>.From(m => m.Id).LessThanOrEqual<Passport>(m => m.PersonId, paAlias).RawSql);

            Assert.Equal("pe.Id <= COUNT(pa.Id)", SqlFilter<Person>.From(p => p.Id).LessThanOrEqual(SqlField<Passport>.Count(p => p.Id)).RawSql);
            var alias = new SqlAlias<Passport>("pasp");
            Assert.Equal("pe.Id <= COUNT(pasp.Id)", SqlFilter<Person>.From(p => p.Id).LessThanOrEqual(SqlField<Passport>.Count(alias, p => p.Id)).ToString());
        }

        [Fact]
        public void In()
        {
            Assert.Equal("pe.Id IN (5,6)", SqlFilter<Person>.From(m => m.Id).In(5, 6).RawSql);
            Assert.Equal("pe.Name IN ('Sergey','Alex')", SqlFilter<Person>.From(m => m.Name).In("Sergey", "Alex").RawSql);
        }

        [Fact]
        public void NotIn()
        {
            Assert.Equal("pe.Id NOT IN (5,6)", SqlFilter<Person>.From(m => m.Id).NotIn(5, 6).RawSql);
            Assert.Equal("pe.Name NOT IN ('Sergey','Alex')", SqlFilter<Person>.From(m => m.Name).NotIn("Sergey", "Alex").RawSql);
        }
    }
}
